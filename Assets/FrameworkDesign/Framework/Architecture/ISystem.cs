
namespace FrameworkDesign
{
    /// <summary>
    /// 系统层接口
    /// </summary>
    /// <remarks>
    /// 定义了系统层的基本功能和行为：标记所属架构、可设置架构、可获取系统、可获取模型、可获取工具、可注册事件、可发送事件。
    /// </remarks>
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent
    {
        /// <summary>
        /// 初始化系统
        /// </summary>
        void Init();
    }

    /// <summary>
    /// 抽象系统类
    /// </summary>
    /// <remarks>
    /// 实现系统层接口的基本功能。
    /// </remarks>
    public abstract class AbstractSystem : ISystem
    {
        // 定义架构对象
        private IArchitecture mArchitecture;

        /// <summary>
        /// 获取架构
        /// </summary>
        /// <returns>返回当前系统的架构</returns>
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        // 显式接口实现：
        // 封装：可以使其只能通过接口来访问，而不能通过类的实例直接访问。这有助于隐藏实现细节，使代码更加封装；
        // 解决命名冲突：如果一个类实现了多个接口，而这些接口中有相同的方法签名，那么显式接口实现可以用来解决这个问题。可以为每个接口提供不同的实现，而不会引起冲突；
        // 清晰的代码结构：显式接口实现还可以使代码结构更清晰，更容易理解哪些方法是为了实现哪个接口。

        /// <summary>
        /// 初始化系统
        /// </summary>
        /// <remarks>
        /// 由具体的子类实现。
        /// </remarks>
        void ISystem.Init()
        {
            OnInit();
        }

        /// <summary>
        /// 设置架构
        /// </summary>
        /// <param name="architecture">要设置的架构对象</param>
        public void SetArchitecture(IArchitecture architecture)
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
