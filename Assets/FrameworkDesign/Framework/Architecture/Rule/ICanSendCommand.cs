
namespace FrameworkDesign
{
    /// <summary>
    /// 可发送命令 接口
    /// </summary>
    /// <remarks>
    /// 命令模式允许将请求封装为对象，从而使发送者和接收者之间解耦。
    /// </remarks>
    public interface ICanSendCommand : IBelongToArchitecture { }

    public static class CanSendCommandExtension
    {
        /// <summary>
        /// 扩展方法：发送命令
        /// </summary>
        /// <typeparam name="T">命令类型，必须实现 ICommand 接口并具有无参数的构造函数</typeparam>
        /// <param name="self">实现 ICanSendCommand 接口的实例</param>
        /// <remarks>
        /// 此方法创建一个新的命令实例并发送。命令的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        /// <summary>
        /// 扩展方法：发送命令
        /// </summary>
        /// <typeparam name="T">命令类型，必须实现 ICommand 接口</typeparam>
        /// <param name="self">实现 ICanSendCommand 接口的实例</param>
        /// <param name="command">命令实例</param>
        /// <remarks>
        /// 此方法发送一个已存在的命令实例。命令的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }
}
