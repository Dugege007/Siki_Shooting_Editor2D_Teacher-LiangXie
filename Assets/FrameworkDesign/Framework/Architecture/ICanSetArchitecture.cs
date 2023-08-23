
namespace FrameworkDesign
{
    /// <summary>
    /// 可设置架构 接口
    /// </summary>
    /// <remarks>
    /// 将架构实例设置到实现此接口的类中，用于确保组件与其所属的架构之间的正确关联。
    /// </remarks>
    public interface ICanSetArchitecture
    {
        /// <summary>
        /// 设置所属的架构
        /// </summary>
        /// <param name="architecture">架构实例</param>
        void SetArchitecture(IArchitecture architecture);
    }
}
