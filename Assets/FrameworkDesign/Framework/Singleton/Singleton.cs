using System;
using System.Reflection;

namespace FrameworkDesign
{
    /// <summary>
    /// 泛型单例工具类
    /// </summary>
    /// <typeparam name="T">单例类的类型</typeparam>
    /// <remarks>
    /// 该类提供了一种通用的单例模式实现。
    /// 通过反射，它可以自动创建和管理任何具有非公共默认构造函数的类的单例实例。
    /// </remarks>
    public class Singleton<T> where T : Singleton<T>
    {
        // 存储单例实例
        private static T mInstance;
        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <value>单例实例</value>
        /// <exception cref="Exception">如果找不到非公共的默认构造函数，则抛出异常</exception>
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var type = typeof(T);

                    // 利用反射，获取 T 的构造函数数组
                    // BindingFlags 绑定标记，Instance 实例，NonPublic 非public的
                    var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

                    // 获取第一个无参构造（默认构造）
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                    if (ctor == null)
                    {
                        // 抛出异常
                        throw new Exception("Non Public Constructor Not Found in + " + type.FullName);
                    }

                    mInstance = ctor.Invoke(null) as T;
                }

                return mInstance;
            }
        }
    }
}
