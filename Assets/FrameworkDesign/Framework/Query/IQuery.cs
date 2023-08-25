
namespace FrameworkDesign
{
    /// <summary>
    /// 查询 接口
    /// </summary>
    /// <typeparam name="TResult">查询结果的类型</typeparam>
    /// <remarks>
    /// 定义了查询的基本功能和行为：标记所属架构、可设置架构、可获取系统、可获取模型、可发送查询；
    /// 用于获取特定的结果。
    /// </remarks>
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanSendQuery
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <returns>返回查询的结果</returns>
        TResult Do();
    }

    /// <summary>
    /// 抽象 查询类
    /// </summary>
    /// <typeparam name="T">查询结果的类型</typeparam>
    /// <remarks>
    /// 实现查询接口的基本功能。
    /// </remarks>
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        // 定义架构对象，用于访问整体架构的功能
        private IArchitecture mArchitecture;

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <returns>返回查询的结果</returns>
        public T Do()
        {
            return OnDo();
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <returns>返回查询的结果</returns>
        /// <remarks>
        /// 在子类中实现具体的查询逻辑。
        /// </remarks>
        protected abstract T OnDo();    // 和命令的写法差不多

        /// <summary>
        /// 获取架构
        /// </summary>
        /// <returns>返回当前查询所属的架构接口</returns>
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        /// <summary>
        /// 设置架构
        /// </summary>
        /// <param name="architecture">要设置的架构对象</param>
        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
}
