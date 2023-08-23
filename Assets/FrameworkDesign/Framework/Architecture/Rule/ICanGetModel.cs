
namespace FrameworkDesign
{
    /// <summary>
    /// 可获取模型 接口
    /// </summary>
    public interface ICanGetModel : IBelongToArchitecture { }

    public static class CanGetModelExtension
    {
        /// <summary>
        /// 扩展方法：从架构中获取模型
        /// </summary>
        /// <typeparam name="T">模型类型，必须实现 IModel 接口</typeparam>
        /// <param name="self">实现 ICanGetModel 接口的实例</param>
        /// <returns>返回指定类型的模型实例</returns>
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
}
