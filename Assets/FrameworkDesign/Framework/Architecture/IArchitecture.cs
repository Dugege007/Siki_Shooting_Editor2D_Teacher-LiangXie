using System;

namespace FrameworkDesign
{
    /// <summary>
    /// 架构接口
    /// </summary>
    public interface IArchitecture
    {
        /// <summary>
        /// 注册系统组件
        /// </summary>
        /// <typeparam name="T">系统类型</typeparam>
        /// <param name="system">系统实例</param>
        void RegisterSystem<T>(T system) where T : ISystem;

        /// <summary>
        /// 注册模型组件
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="model">模型实例</param>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// 注册工具组件
        /// </summary>
        /// <typeparam name="T">工具类型</typeparam>
        /// <param name="utility">工具实例</param>
        void RegisterUtility<T>(T utility) where T : IUtility;

        /// <summary>
        /// 获取系统组件
        /// </summary>
        /// <typeparam name="T">系统类型</typeparam>
        /// <returns>系统实例</returns>
        T GetSystem<T>() where T : class, ISystem;

        /// <summary>
        /// 获取模型组件
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <returns>模型实例</returns>
        T GetModel<T>() where T : class, IModel;

        /// <summary>
        /// 获取工具组件
        /// </summary>
        /// <typeparam name="T">工具类型</typeparam>
        /// <returns>工具实例</returns>
        T GetUtility<T>() where T : class, IUtility;

        // 在软件架构和设计模式中，术语“组件”通常用来描述一个独立的、可重用的代码单元
        // 组件是一种封装了特定功能或行为的结构，它与其他组件协同工作，共同构成一个完整的系统
        // 与 Unity 游戏物体的组件在游戏物体上的作用类似

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        void SendCommand<T>() where T : ICommand, new();

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        /// <param name="command">命令实例</param>
        void SendCommand<T>(T command) where T : ICommand;

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="e">事件实例</param>
        void SendEvent<T>(T e);

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">事件处理方法</param>
        /// <returns>返回注销接口</returns>
        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">事件处理方法</param>
        void UnRegisterEvent<T>(Action<T> onEvent);
    }
}
