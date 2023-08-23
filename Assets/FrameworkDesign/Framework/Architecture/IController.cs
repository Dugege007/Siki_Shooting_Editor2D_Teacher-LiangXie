
namespace FrameworkDesign
{
    /// <summary>
    /// 表现层控制器接口
    /// </summary>
    /// <remarks>
    /// 该接口定义了表现层控制器的基本功能和行为：标记所属架构、可获取系统、可获取模型、可发送命令、可注册事件；
    /// 由于表现层对象经常进行创建和销毁，因此将表现层对象注册到架构中没有意义；
    /// 通过实现此接口，可以标记表现层对象，并使其能够访问架构中的系统或模型，而无需使用单例形式获取。
    /// </remarks>
    public interface IController : IBelongToArchitecture, ICanGetSystem,  ICanGetModel, ICanSendCommand, ICanRegisterEvent 
    {
        // 此接口主要用于组合上述功能，不需要额外的方法定义
    }
}
