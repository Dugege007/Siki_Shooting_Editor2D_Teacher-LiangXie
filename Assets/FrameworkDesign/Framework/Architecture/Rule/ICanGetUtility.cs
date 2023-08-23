
namespace FrameworkDesign
{
    /// <summary>
    /// 可获取工具 接口
    /// </summary>
    public interface ICanGetUtility : IBelongToArchitecture { }

    public static class CanGetUtilityExtension
    {
        /// <summary>
        /// 扩展方法：从架构中获取工具
        /// </summary>
        /// <typeparam name="T">工具类型，必须实现 IUtility 接口</typeparam>
        /// <param name="self">实现 ICanGetUtility 接口的实例</param>
        /// <returns>返回指定类型的工具实例</returns>
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
}
