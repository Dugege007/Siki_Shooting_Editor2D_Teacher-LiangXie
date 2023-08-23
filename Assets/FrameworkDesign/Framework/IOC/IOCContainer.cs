using System;
using System.Collections.Generic;

namespace FrameworkDesign
{
    /// <summary>
    /// IOC容器 (Inversion of Control 控制反转)
    /// 也称依赖注入（Dependency Injection，DI）
    /// </summary>
    /// <remarks>
    /// IOC容器用于管理对象的创建和生命周期，允许将依赖关系从代码中解耦；
    /// 通过此容器，可以注册和检索单例对象，从而实现跨组件的共享和重用。
    /// </remarks>
    public class IOCContainer
    {
        // 存储已注册的单例对象
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="instance">单例对象实例</param>
        /// <remarks>
        /// 如果该类型已注册，则会更新现有实例。
        /// </remarks>
        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        /// <summary>
        /// 获取已注册的单例对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>返回单例对象实例，如果未注册则返回 null</returns>
        public T Get<T>() where T : class
        {
            var key = typeof(T);

            object retInstance;
            if (mInstances.TryGetValue(key, out retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }
}
