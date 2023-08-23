
namespace FrameworkDesign
{
    /// <summary>
    /// 模型层接口
    /// </summary>
    /// <remarks>
    /// 定义了模型层的基本功能和行为：标记所属架构、可设置架构、可获取工具、可发送事件。
    /// </remarks>
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        /// <summary>
        /// 初始化模型
        /// </summary>
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        // 定义架构对象
        private IArchitecture mArchitecture;

        /// <summary>
        /// 获取架构
        /// </summary>
        /// <returns>返回当前模型的架构</returns>
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        /// <summary>
        /// 初始化模型
        /// </summary>
        /// <remarks>
        /// 由具体的子类实现。
        /// </remarks>
        void IModel.Init()
        {
            OnInit();
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
        /// 初始化
        /// </summary>
        /// <remarks>
        /// 由具体的子类实现。
        /// </remarks>
        protected abstract void OnInit();
    }
}
