using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{
    /// <summary>
    /// 基于类型的事件机制 接口
    /// </summary>
    /// <remarks>
    /// 定义了发送、注册和注销事件的方法。
    /// </remarks>
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }

    // 由于注册事件后，在销毁物体时要注销对象，步骤比较繁琐
    // 为了简化操作，引入一个 IUnRegister 接口

    /// <summary>
    /// 注销事件 接口
    /// </summary>
    /// <remarks>
    /// 提供注销事件的方法。
    /// </remarks>
    public interface IUnRegister
    {
        void UnRegister();
    }

    // 使用结构体会好一些...

    /// <summary>
    /// 注销事件 结构体
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;

        public void UnRegister()
        {
            TypeEventSystem.UnRegister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    /// <summary>
    /// 注销事件的触发器
    /// </summary>
    /// <remarks>
    /// 当挂载的游戏物体销毁时，会自动注销所有注册的事件。
    /// </remarks>
    public class UnRegisterDestoryTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    // 为了简化使用...
    public static class UnRegisterExtension
    {
        /// <summary>
        /// 扩展方法：为在销毁时需要注销自身所有事件的物体，添加注销事件的触发器
        /// </summary>
        /// <param name="unRegister">要注销的事件</param>
        /// <param name="obj">游戏物体</param>
        /// <remarks>
        /// 用于简化注销事件的操作。
        /// </remarks>
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject obj)
        {
            var trigger = obj.GetComponent<UnRegisterDestoryTrigger>();

            if (trigger == null)
            {
                trigger = obj.AddComponent<UnRegisterDestoryTrigger>();
            }

            trigger.AddUnRegister(unRegister);
        }
    }

    /// <summary>
    /// 基于类型的事件机制实现类
    /// </summary>
    /// <remarks>
    /// 提供了注册、发送和注销事件的功能。
    /// </remarks>
    public class TypeEventSystem : ITypeEventSystem
    {
        // 事件机制本身就是一个数据结构，类似字典

        // 定义一个接口，用于注册事件
        // 通过 Register 方法完成一次注册，这个接口会有多次注册
        // 一个消息的关键字会有多次注册
        public interface IRegistrations { }

        /// <summary>
        /// 注册特定类型的事件 泛型类
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }

        // 使用字典存储所有注册的事件
        Dictionary<Type, IRegistrations> mEventRegistration = new Dictionary<Type, IRegistrations>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">事件处理方法</param>
        /// <returns>返回一个可以用于注销的接口</returns>
        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations) == false)
            {
                registrations = new Registrations<T>();
                mEventRegistration.Add(type, registrations);
            }

            (registrations as Registrations<T>).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型，必须有无参构造函数</typeparam>
        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="e">事件实例</param>
        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent(e);
            }
        }

        /// <summary>
        /// 注销过程
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">要注销的事件方法</param>
        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
}
