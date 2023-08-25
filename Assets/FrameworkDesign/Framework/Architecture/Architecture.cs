using ShootingEditor2D;
using System;
using System.Collections.Generic;

namespace FrameworkDesign
{
    /// <summary>
    /// 架构类
    /// </summary>
    /// <typeparam name="T">架构类型</typeparam>
    /// <remarks>
    /// 构建“使用 IOC 容器时要重复定义的代码”的泛型基类。
    /// </remarks>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()  // 和 Singleton 泛型单例写法一样
    {
        // 是否初始化完成
        private bool mInited = false;
        // 缓存需要要初始化的 Model，在 MakeSureArchitecture() 中统一初始化
        private List<IModel> mModels = new List<IModel>();
        // 缓存需要初始化的System
        private List<ISystem> mSystems = new List<ISystem>();
        // IOC容器
        private IOCContainer mContainer = new IOCContainer();

        /// <summary>
        /// 注册补丁操作
        /// </summary>
        public static Action<T> OnRegisterPatch = architecture => { };

        // 单例架构实例
        private static T mArchitecture;
        // 事件系统
        private ITypeEventSystem mTypeEventSystem= new TypeEventSystem();

        /// <summary>
        /// 获取架构接口
        /// </summary>
        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null)
                {
                    MakeSureArchitecture();
                }

                return mArchitecture;
            }
        }

        /// <summary>
        /// 确保对象存在
        /// </summary>
        private static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                OnRegisterPatch?.Invoke(mArchitecture);

                // 把缓存的模型都初始化
                //模型层比系统更加底层，所以系统可以直接访问模型层
                // 所以应确保模型层先初始化
                foreach (var model in mArchitecture.mModels)
                {
                    model.Init();
                }

                mArchitecture.mModels.Clear();

                //系统层初始化时可能会访问模型层
                foreach (var system in mArchitecture.mSystems)
                {
                    system.Init();
                }

                mArchitecture.mSystems.Clear();

                mArchitecture.mInited = true;
            }
        }

        /// <summary>
        /// 抽象初始化方法
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 注册模型组件
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="modelInstance">模型实例</param>
        /// <remarks>
        /// 模型组件在软件架构中通常用来表示、管理数据和业务逻辑的状态；
        /// 主要用于：数据封装、业务逻辑、状态管理、通知变更、与其他组件的解耦。
        /// </remarks>
        public void RegisterModel<T>(T modelInstance) where T : IModel
        {
            // 设置架构
            modelInstance.SetArchitecture(this);
            // 注册到 IOC 容器
            mContainer.Register<T>(modelInstance);

            // 如果架构还未初始化，将Model组件添加到缓存列表中，稍后统一初始化
            if (!mInited)
                mModels.Add(modelInstance);
            // 如果架构已初始化，直接初始化Model组件
            else
                modelInstance.Init();
        }

        /// <summary>
        /// 注册工具组件
        /// </summary>
        /// <typeparam name="T">工具类型</typeparam>
        /// <param name="utilityInstance">工具实例</param>
        /// <remarks>
        /// 工具组件是用于提供各种实用工具的组件，例如日志、配置读取等。
        /// </remarks>
        public void RegisterUtility<T>(T utilityInstance) where T : IUtility
        {
            // 注册到 IOC 容器
            mContainer.Register<T>(utilityInstance);
        }

        /// <summary>
        /// 注册系统组件
        /// </summary>
        /// <typeparam name="T">系统类型</typeparam>
        /// <param name="systemInstance">系统实例</param>
        /// <remarks>
        /// 系统组件通常负责处理业务逻辑和协调其他组件。
        /// </remarks>
        public void RegisterSystem<T>(T systemInstance) where T : ISystem
        {
            // 设置架构
            systemInstance.SetArchitecture(this);
            // 注册到IOC容器
            mContainer.Register<T>(systemInstance);

            // 如果架构还未初始化，将System组件添加到缓存列表中，稍后统一初始化
            if (!mInited)
                mSystems.Add(systemInstance);
            // 如果架构已初始化，直接初始化System组件
            else
                systemInstance.Init();
        }

        /// <summary>
        /// 获取模型组件
        /// </summary>
        /// <typeparam name="T">模型类型，必须实现 I模型接口</typeparam>
        /// <returns>返回指定类型的模型实例</returns>
        public T GetModel<T>() where T : class, IModel
        {
            // 从 IOC 容器中获取指定类型的模型组件实例
            return mContainer.Get<T>();
        }

        /// <summary>
        /// 获取工具组件
        /// </summary>
        /// <typeparam name="T">工具类型，必须实现 I工具接口</typeparam>
        /// <returns>返回指定类型的工具实例</returns>
        public T GetUtility<T>() where T : class, IUtility
        {
            // 从 IOC 容器中获取指定类型的工具组件实例
            return mContainer.Get<T>();
        }

        /// <summary>
        /// 获取系统组件
        /// </summary>
        /// <typeparam name="T">系统类型，必须实现 I系统接口</typeparam>
        /// <returns>返回指定类型的系统实例</returns>
        public T GetSystem<T>() where T : class, ISystem
        {
            // 从 IOC 容器中获取指定类型的系统组件实例
            return mContainer.Get<T>();
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="T">命令类型，必须实现 ICommand 接口，并具有无参数的构造函数</typeparam>
        /// <remarks>
        /// 命令模式用于封装请求作为对象，从而允许用户参数化请求、排队请求、并提供其他功能。
        /// </remarks>
        public void SendCommand<T>() where T : ICommand, new()
        {
            // 创建指定类型的命令实例
            var command = new T();
            // 设置其架构为当前架构
            command.SetArchitecture(this);
            // 执行命令
            command.Execute();
            // 执行完后清除命令与架构之间的引用，避免不必要的引用持有
            command.SetArchitecture(null);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="T">命令类型，必须实现 ICommand 接口</typeparam>
        /// <param name="command">要执行的命令实例</param>
        /// <remarks>
        /// 命令模式用于封装请求作为对象，从而允许用户参数化请求、排队请求、并提供其他功能。
        /// </remarks>
        public void SendCommand<T>(T command) where T : ICommand
        {
            // 设置其架构为当前架构
            command.SetArchitecture(this);
            // 执行命令
            command.Execute();
        }

        /// <summary>
        /// 发送查询请求
        /// </summary>
        /// <typeparam name="TResult">查询结果的类型</typeparam>
        /// <param name="query">实现了 IQuery 接口的查询对象</param>
        /// <returns>返回查询的结果</returns>
        /// <remarks>
        /// 该方法允许发送一个查询请求，并获取查询的结果；
        /// 后续可在该方法中添加更多查询规则。
        /// </remarks>
        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型，必须具有无参数的构造函数</typeparam>
        /// <remarks>
        /// 事件系统允许组件之间进行松耦合的通信，使得事件的发送者和接收者不必直接相互引用。
        /// </remarks>
        public void SendEvent<T>() where T : new()
        {
            // 发送指定类型的事件
            mTypeEventSystem.Send<T>();
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="e">事件实例</param>
        /// <remarks>
        /// 事件系统允许组件之间进行松耦合的通信，使得事件的发送者和接收者不必直接相互引用。
        /// </remarks>
        public void SendEvent<T>(T e)
        {
            // 发送指定类型的事件
            mTypeEventSystem.Send<T>(e);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">事件处理的委托方法</param>
        /// <returns>返回一个注销接口，可用于注销此事件</returns>
        public IUnRegister RegisterEvent<T>(Action<T> onEvent)
        {
            return mTypeEventSystem.Register<T>(onEvent);
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">要注销的事件处理的委托方法</param>
        public void UnRegisterEvent<T>(Action<T> onEvent)
        {
            mTypeEventSystem.UnRegister<T>(onEvent);
        }
    }
}
