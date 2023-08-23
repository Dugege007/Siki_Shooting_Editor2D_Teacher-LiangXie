
namespace FrameworkDesign
{
    /// <summary>
    /// 标记属于哪个架构 接口
    /// </summary>
    /// <remarks>
    /// 由于 Architecture 中的 System 和 Model 会相互调用，所以我们需要一个接口来标记这个 System 或者 Model 是属于哪个 Architecture 的；
    /// 解决 Architecture 中的递归调用。
    /// </remarks>
    public interface IBelongToArchitecture
    {
        /// <summary>
        /// 获取所属的架构
        /// </summary>
        /// <returns>返回所属的架构实例</returns>
        IArchitecture GetArchitecture();
    }
}
