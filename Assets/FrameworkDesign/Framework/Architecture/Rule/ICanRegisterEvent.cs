using System;

namespace FrameworkDesign
{
    /// <summary>
    /// 可注册、注销事件 接口
    /// </summary>
    /// <remarks>
    /// 事件是一种消息机制，用于在不同组件之间传递信息。
    /// </remarks>
    public interface ICanRegisterEvent : IBelongToArchitecture { }

    public static class CanRegisterEventExtension
    {
        /// <summary>
        /// 扩展方法：注册事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="self">实现 ICanRegisterEvent 接口的实例</param>
        /// <param name="onEvent">事件处理的回调方法</param>
        /// <returns>返回一个 IUnRegister 接口，可以用于之后的注销操作</returns>
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }

        /// <summary>
        /// 扩展方法：注销事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="self">实现 ICanRegisterEvent 接口的实例</param>
        /// <param name="onEvent">事件处理的回调方法</param>
        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }
}
