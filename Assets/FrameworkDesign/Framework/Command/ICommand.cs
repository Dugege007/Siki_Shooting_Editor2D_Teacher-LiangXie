
namespace FrameworkDesign
{
    /// <summary>
    /// 命令接口
    /// </summary>
    /// <remarks>
    /// 用于处理交互逻辑，分担控制器的交互逻辑职责；
    /// 基本功能和行为：标记所属架构、可设置架构、可获取系统、可获取模型、可获取工具、可发送命令、可发送事件；
    /// 通过将混乱的交互逻辑代码从控制器迁移到命令中，使代码结构更清晰。
    /// </remarks>
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanSendEvent
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// 抽象命令类
    /// </summary>
    /// <remarks>
    /// 实现命令接口的基本功能，提供执行命令的基础结构。
    /// </remarks>
    public abstract class AbstractCommand : ICommand
    {
        // 定义架构对象
        private IArchitecture mArchitecture;

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <remarks>
        /// 由具体的子类实现。
        /// </remarks>
        void ICommand.Execute()
        {
            OnExecute();
        }

        /// <summary>
        /// 获取架构
        /// </summary>
        /// <returns>返回当前命令的架构</returns>
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        /// <summary>
        /// 设置架构
        /// </summary>
        /// <param name="architecture">要设置的架构对象</param>
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <remarks>
        /// 由具体的子类实现。
        /// </remarks>
        protected virtual void OnExecute() { }
    }
}
