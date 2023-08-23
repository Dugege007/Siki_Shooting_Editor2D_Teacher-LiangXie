
namespace FrameworkDesign
{
    /// <summary>
    /// 可获取系统 接口
    /// </summary>
    public interface ICanGetSystem : IBelongToArchitecture { }

    public static class CanGetSystemExtension
    {
        /// <summary>
        /// 扩展方法：从架构中获取系统
        /// </summary>
        /// <typeparam name="T">系统类型，必须实现 ISystem 接口</typeparam>
        /// <param name="self">实现 ICanGetSystem 接口的实例</param>
        /// <returns>返回指定类型的系统实例</returns>
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
}
