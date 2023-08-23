
namespace FrameworkDesign
{
    /// <summary>
    /// 可发送事件 接口
    /// </summary>
    /// <remarks>
    /// 事件是一种消息机制，用于在不同组件之间传递信息。
    /// </remarks>
    public interface ICanSendEvent : IBelongToArchitecture { }

    public static class CanSendEventExtension
    {
        /// <summary>
        /// 扩展方法：发送事件
        /// </summary>
        /// <typeparam name="T">事件类型，必须具有无参数的构造函数</typeparam>
        /// <param name="self">实现 ICanSendEvent 接口的实例</param>
        /// <remarks>
        /// 此方法创建一个新的事件实例并发送。事件的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        /// <summary>
        /// 扩张方法：发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="self">实现 ICanSendEvent 接口的实例</param>
        /// <param name="e">事件实例</param>
        /// <remarks>
        /// 此方法发送一个已存在的事件实例。事件的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }
}
