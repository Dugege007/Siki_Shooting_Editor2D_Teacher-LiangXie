using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace QFramework
{
    #region Architeture
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
        /// 发送查询
        /// </summary>
        /// <typeparam name="TResult">查询返回的结果类型</typeparam>
        /// <param name="query">查询实例，实现了IQuery接口的对象</param>
        /// <returns>返回查询的结果</returns>
        TResult SendQuery<TResult>(IQuery<TResult> query);

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

    /// <summary>
    /// 架构类
    /// </summary>
    /// <typeparam name="T">架构类型</typeparam>
    /// <remarks>
    /// 构建“使用 IOC 容器时要重复定义的代码”的泛型基类。
    /// </remarks>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()  // 和 Singleton 泛型单例写法一样
    {
        // 是否初始化完成
        private bool mInited = false;
        // 缓存需要要初始化的 Model，在 MakeSureArchitecture() 中统一初始化
        private List<IModel> mModels = new List<IModel>();
        // 缓存需要初始化的System
        private List<ISystem> mSystems = new List<ISystem>();
        // IOC容器
        private IOCContainer mContainer = new IOCContainer();

        /// <summary>
        /// 注册补丁操作
        /// </summary>
        public static Action<T> OnRegisterPatch = architecture => { };

        // 单例架构实例
        private static T mArchitecture;
        // 事件系统
        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        /// <summary>
        /// 获取架构接口
        /// </summary>
        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null)
                {
                    MakeSureArchitecture();
                }

                return mArchitecture;
            }
        }

        /// <summary>
        /// 确保对象存在
        /// </summary>
        private static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                OnRegisterPatch?.Invoke(mArchitecture);

                // 把缓存的模型都初始化
                //模型层比系统更加底层，所以系统可以直接访问模型层
                // 所以应确保模型层先初始化
                foreach (var model in mArchitecture.mModels)
                {
                    model.Init();
                }

                mArchitecture.mModels.Clear();

                //系统层初始化时可能会访问模型层
                foreach (var system in mArchitecture.mSystems)
                {
                    system.Init();
                }

                mArchitecture.mSystems.Clear();

                mArchitecture.mInited = true;
            }
        }

        /// <summary>
        /// 抽象初始化方法
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 注册模型组件
        /// </summary>
        /// <typeparam name="TModel">模型类型</typeparam>
        /// <param name="modelInstance">模型实例</param>
        /// <remarks>
        /// 模型组件在软件架构中通常用来表示、管理数据和业务逻辑的状态；
        /// 主要用于：数据封装、业务逻辑、状态管理、通知变更、与其他组件的解耦。
        /// </remarks>
        public void RegisterModel<TModel>(TModel modelInstance) where TModel : IModel
        {
            // 设置架构
            modelInstance.SetArchitecture(this);
            // 注册到 IOC 容器
            mContainer.Register<TModel>(modelInstance);

            // 如果架构还未初始化，将Model组件添加到缓存列表中，稍后统一初始化
            if (!mInited)
                mModels.Add(modelInstance);
            // 如果架构已初始化，直接初始化Model组件
            else
                modelInstance.Init();
        }

        /// <summary>
        /// 注册工具组件
        /// </summary>
        /// <typeparam name="TUtility">工具类型</typeparam>
        /// <param name="utilityInstance">工具实例</param>
        /// <remarks>
        /// 工具组件是用于提供各种实用工具的组件，例如日志、配置读取等。
        /// </remarks>
        public void RegisterUtility<TUtility>(TUtility utilityInstance) where TUtility : IUtility
        {
            // 注册到 IOC 容器
            mContainer.Register<TUtility>(utilityInstance);
        }

        /// <summary>
        /// 注册系统组件
        /// </summary>
        /// <typeparam name="TSystem">系统类型</typeparam>
        /// <param name="systemInstance">系统实例</param>
        /// <remarks>
        /// 系统组件通常负责处理业务逻辑和协调其他组件。
        /// </remarks>
        public void RegisterSystem<TSystem>(TSystem systemInstance) where TSystem : ISystem
        {
            // 设置架构
            systemInstance.SetArchitecture(this);
            // 注册到IOC容器
            mContainer.Register<TSystem>(systemInstance);

            // 如果架构还未初始化，将System组件添加到缓存列表中，稍后统一初始化
            if (!mInited)
                mSystems.Add(systemInstance);
            // 如果架构已初始化，直接初始化System组件
            else
                systemInstance.Init();
        }

        /// <summary>
        /// 获取模型组件
        /// </summary>
        /// <typeparam name="TModel">模型类型，必须实现 I模型接口</typeparam>
        /// <returns>返回指定类型的模型实例</returns>
        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            // 从 IOC 容器中获取指定类型的模型组件实例
            return mContainer.Get<TModel>();
        }

        /// <summary>
        /// 获取工具组件
        /// </summary>
        /// <typeparam name="TUtility">工具类型，必须实现 I工具接口</typeparam>
        /// <returns>返回指定类型的工具实例</returns>
        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            // 从 IOC 容器中获取指定类型的工具组件实例
            return mContainer.Get<TUtility>();
        }

        /// <summary>
        /// 获取系统组件
        /// </summary>
        /// <typeparam name="TSystem">系统类型，必须实现 I系统接口</typeparam>
        /// <returns>返回指定类型的系统实例</returns>
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            // 从 IOC 容器中获取指定类型的系统组件实例
            return mContainer.Get<TSystem>();
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="TCommand">命令类型，必须实现 ICommand 接口，并具有无参数的构造函数</typeparam>
        /// <remarks>
        /// 命令模式用于封装请求作为对象，从而允许用户参数化请求、排队请求、并提供其他功能。
        /// </remarks>
        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            // 创建指定类型的命令实例
            var command = new TCommand();
            // 设置其架构为当前架构
            command.SetArchitecture(this);
            // 执行命令
            command.Execute();
            // 执行完后清除命令与架构之间的引用，避免不必要的引用持有
            command.SetArchitecture(null);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="TCommand">命令类型，必须实现 ICommand 接口</typeparam>
        /// <param name="command">要执行的命令实例</param>
        /// <remarks>
        /// 命令模式用于封装请求作为对象，从而允许用户参数化请求、排队请求、并提供其他功能。
        /// </remarks>
        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            // 设置其架构为当前架构
            command.SetArchitecture(this);
            // 执行命令
            command.Execute();
        }

        /// <summary>
        /// 发送查询请求
        /// </summary>
        /// <typeparam name="TResult">查询结果的类型</typeparam>
        /// <param name="query">实现了 IQuery 接口的查询对象</param>
        /// <returns>返回查询的结果</returns>
        /// <remarks>
        /// 该方法允许发送一个查询请求，并获取查询的结果；
        /// 后续可在该方法中添加更多查询规则。
        /// </remarks>
        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型，必须具有无参数的构造函数</typeparam>
        /// <remarks>
        /// 事件系统允许组件之间进行松耦合的通信，使得事件的发送者和接收者不必直接相互引用。
        /// </remarks>
        public void SendEvent<TEvent>() where TEvent : new()
        {
            // 发送指定类型的事件
            mTypeEventSystem.Send<TEvent>();
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="e">事件实例</param>
        /// <remarks>
        /// 事件系统允许组件之间进行松耦合的通信，使得事件的发送者和接收者不必直接相互引用。
        /// </remarks>
        public void SendEvent<TEvent>(TEvent e)
        {
            // 发送指定类型的事件
            mTypeEventSystem.Send<TEvent>(e);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="onEvent">事件处理的委托方法</param>
        /// <returns>返回一个注销接口，可用于注销此事件</returns>
        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="onEvent">要注销的事件处理的委托方法</param>
        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }
    }
    #endregion

    #region Controller
    /// <summary>
    /// 表现层控制器接口
    /// </summary>
    /// <remarks>
    /// 该接口定义了表现层控制器的基本功能和行为：标记所属架构、可获取系统、可获取模型、可发送命令、可注册事件、可发送查询；
    /// 由于表现层对象经常进行创建和销毁，因此将表现层对象注册到架构中没有意义；
    /// 通过实现此接口，可以标记表现层对象，并使其能够访问架构中的系统或模型，而无需使用单例形式获取。
    /// </remarks>
    public interface IController : IBelongToArchitecture, ICanGetSystem, ICanGetModel, ICanSendCommand, ICanRegisterEvent, ICanSendQuery
    {
        // 此接口主要用于组合上述功能，不需要额外的方法定义
    }
    #endregion

    #region System
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
    #endregion

    #region Model
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
    #endregion

    #region Utility
    /// <summary>
    /// 工具层接口
    /// </summary>
    public interface IUtility
    {
        // 此接口不需要额外的方法定义
    }
    #endregion

    #region Command
    /// <summary>
    /// 命令接口
    /// </summary>
    /// <remarks>
    /// 用于处理交互逻辑，分担控制器的交互逻辑职责；
    /// 基本功能和行为：标记所属架构、可设置架构、可获取系统、可获取模型、可获取工具、可发送命令、可发送事件、可发送查询；
    /// 通过将混乱的交互逻辑代码从控制器迁移到命令中，使代码结构更清晰。
    /// </remarks>
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// 抽象命令类
    /// </summary>
    /// <remarks>
    /// 实现命令接口的基本功能，提供执行命令的基础结构。
    /// </remarks>
    public abstract class AbstractCommand : ICommand
    {
        // 定义架构对象
        private IArchitecture mArchitecture;

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <remarks>
        /// 由具体的子类实现。
        /// </remarks>
        void ICommand.Execute()
        {
            OnExecute();
        }

        /// <summary>
        /// 获取架构
        /// </summary>
        /// <returns>返回当前命令的架构</returns>
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
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
        /// 执行命令
        /// </summary>
        /// <remarks>
        /// 由具体的子类实现。
        /// </remarks>
        protected virtual void OnExecute() { }
    }
    #endregion

    #region Query
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
    #endregion

    #region Rule
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

    /// <summary>
    /// 可注册、注销事件 接口
    /// </summary>
    /// <remarks>
    /// 事件是一种消息机制，用于在不同组件之间传递信息。
    /// </remarks>
    public interface ICanRegisterEvent : IBelongToArchitecture { }

    public static class CanRegisterEventExtension
    {
        /// <summary>
        /// 扩展方法：注册事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="self">实现 ICanRegisterEvent 接口的实例</param>
        /// <param name="onEvent">事件处理的回调方法</param>
        /// <returns>返回一个 IUnRegister 接口，可以用于之后的注销操作</returns>
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }

        /// <summary>
        /// 扩展方法：注销事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="self">实现 ICanRegisterEvent 接口的实例</param>
        /// <param name="onEvent">事件处理的回调方法</param>
        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }

    /// <summary>
    /// 可发送事件 接口
    /// </summary>
    /// <remarks>
    /// 事件是一种消息机制，用于在不同组件之间传递信息。
    /// </remarks>
    public interface ICanSendEvent : IBelongToArchitecture { }

    public static class CanSendEventExtension
    {
        /// <summary>
        /// 扩展方法：发送事件
        /// </summary>
        /// <typeparam name="T">事件类型，必须具有无参数的构造函数</typeparam>
        /// <param name="self">实现 ICanSendEvent 接口的实例</param>
        /// <remarks>
        /// 此方法创建一个新的事件实例并发送。事件的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        /// <summary>
        /// 扩张方法：发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="self">实现 ICanSendEvent 接口的实例</param>
        /// <param name="e">事件实例</param>
        /// <remarks>
        /// 此方法发送一个已存在的事件实例。事件的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }

    /// <summary>
    /// 可发送命令 接口
    /// </summary>
    /// <remarks>
    /// 命令模式允许将请求封装为对象，从而使发送者和接收者之间解耦。
    /// </remarks>
    public interface ICanSendCommand : IBelongToArchitecture { }

    public static class CanSendCommandExtension
    {
        /// <summary>
        /// 扩展方法：发送命令
        /// </summary>
        /// <typeparam name="T">命令类型，必须实现 ICommand 接口并具有无参数的构造函数</typeparam>
        /// <param name="self">实现 ICanSendCommand 接口的实例</param>
        /// <remarks>
        /// 此方法创建一个新的命令实例并发送。命令的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        /// <summary>
        /// 扩展方法：发送命令
        /// </summary>
        /// <typeparam name="T">命令类型，必须实现 ICommand 接口</typeparam>
        /// <param name="self">实现 ICanSendCommand 接口的实例</param>
        /// <param name="command">命令实例</param>
        /// <remarks>
        /// 此方法发送一个已存在的命令实例。命令的具体逻辑由所属架构处理。
        /// </remarks>
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }

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

    public interface ICanSendQuery : IBelongToArchitecture { }

    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
    #endregion

    #region TypeEventSystem
    /// <summary>
    /// 基于类型的事件机制 接口
    /// </summary>
    /// <remarks>
    /// 定义了发送、注册和注销事件的方法。
    /// </remarks>
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }

    // 由于注册事件后，在销毁物体时要注销对象，步骤比较繁琐
    // 为了简化操作，引入一个 IUnRegister 接口

    /// <summary>
    /// 注销事件 接口
    /// </summary>
    /// <remarks>
    /// 提供注销事件的方法。
    /// </remarks>
    public interface IUnRegister
    {
        void UnRegister();
    }

    // 使用结构体会好一些...

    /// <summary>
    /// 注销事件 结构体
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;

        public void UnRegister()
        {
            TypeEventSystem.UnRegister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    /// <summary>
    /// 注销事件的触发器
    /// </summary>
    /// <remarks>
    /// 当挂载的游戏物体销毁时，会自动注销所有注册的事件。
    /// </remarks>
    public class UnRegisterDestoryTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    // 为了简化使用...
    public static class UnRegisterExtension
    {
        /// <summary>
        /// 扩展方法：为在销毁时需要注销自身所有事件的物体，添加注销事件的触发器
        /// </summary>
        /// <param name="unRegister">要注销的事件</param>
        /// <param name="obj">游戏物体</param>
        /// <remarks>
        /// 用于简化注销事件的操作。
        /// </remarks>
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject obj)
        {
            var trigger = obj.GetComponent<UnRegisterDestoryTrigger>();

            if (trigger == null)
            {
                trigger = obj.AddComponent<UnRegisterDestoryTrigger>();
            }

            trigger.AddUnRegister(unRegister);
        }
    }

    /// <summary>
    /// 基于类型的事件机制实现类
    /// </summary>
    /// <remarks>
    /// 提供了注册、发送和注销事件的功能。
    /// </remarks>
    public class TypeEventSystem : ITypeEventSystem
    {
        // 事件机制本身就是一个数据结构，类似字典

        // 定义一个接口，用于注册事件
        // 通过 Register 方法完成一次注册，这个接口会有多次注册
        // 一个消息的关键字会有多次注册
        public interface IRegistrations { }

        /// <summary>
        /// 注册特定类型的事件 泛型类
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }

        // 使用字典存储所有注册的事件
        Dictionary<Type, IRegistrations> mEventRegistration = new Dictionary<Type, IRegistrations>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">事件处理方法</param>
        /// <returns>返回一个可以用于注销的接口</returns>
        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations) == false)
            {
                registrations = new Registrations<T>();
                mEventRegistration.Add(type, registrations);
            }

            (registrations as Registrations<T>).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型，必须有无参构造函数</typeparam>
        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="e">事件实例</param>
        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent(e);
            }
        }

        /// <summary>
        /// 注销过程
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="onEvent">要注销的事件方法</param>
        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
    #endregion

    #region IOC
    /// <summary>
    /// IOC容器 (Inversion of Control 控制反转)
    /// 也称依赖注入（Dependency Injection，DI）
    /// </summary>
    /// <remarks>
    /// IOC容器用于管理对象的创建和生命周期，允许将依赖关系从代码中解耦；
    /// 通过此容器，可以注册和检索单例对象，从而实现跨组件的共享和重用。
    /// </remarks>
    public class IOCContainer
    {
        // 存储已注册的单例对象
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        /// <summary>
        /// 注册单例对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="instance">单例对象实例</param>
        /// <remarks>
        /// 如果该类型已注册，则会更新现有实例。
        /// </remarks>
        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        /// <summary>
        /// 获取已注册的单例对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>返回单例对象实例，如果未注册则返回 null</returns>
        public T Get<T>() where T : class
        {
            var key = typeof(T);

            object retInstance;
            if (mInstances.TryGetValue(key, out retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }
    #endregion

    #region Singleton
    /// <summary>
    /// 泛型单例工具类
    /// </summary>
    /// <typeparam name="T">单例类的类型</typeparam>
    /// <remarks>
    /// 该类提供了一种通用的单例模式实现。
    /// 通过反射，它可以自动创建和管理任何具有非公共默认构造函数的类的单例实例。
    /// </remarks>
    public class Singleton<T> where T : Singleton<T>
    {
        // 存储单例实例
        private static T mInstance;

        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <value>单例实例</value>
        /// <exception cref="Exception">如果找不到非公共的默认构造函数，则抛出异常</exception>
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var type = typeof(T);

                    // 利用反射，获取 T 的构造函数数组
                    // BindingFlags 绑定标记，Instance 实例，NonPublic 非public的
                    var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

                    // 获取第一个无参构造（默认构造）
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                    if (ctor == null)
                    {
                        // 抛出异常
                        throw new Exception("Non Public Constructor Not Found in + " + type.FullName);
                    }

                    mInstance = ctor.Invoke(null) as T;
                }

                return mInstance;
            }
        }
    }
    #endregion

    #region BindableProperty
    // BindableProperty 可绑定的属性的简单实现
    // 用于可比较的值的更新
    // 数据 + 数据变更事件 的合体，既存储数据，又充当C#中的属性角色，也可以让别的地方监听它数据的变更事件，这样会大量减少样板代码；
    // 如再加个金币功能就要写一套一模一样的代码，比如增加一个 mGold、OnGoldChanged 委托、Gold 的数值比较逻辑，这种样板代码；
    // 所以很多框架都会用代码生成或者泛型方式去减少样板代码的编写时间；
    // 自底向上（如子节点通知父节点）的逻辑关系可以使用委托或事件，自顶向下（如父节点调用子节点）的逻辑关系可以使用方法调用

    /// <summary>
    /// 可绑定属性的简单实现，用于可比较的值的更新
    /// </summary>
    /// <typeparam name="T">比较的类型</typeparam>
    /// <remarks>
    /// 数据 + 数据变更事件的合体，既存储数据，又充当C#中的属性角色，也可以让别的地方监听它数据的变更事件。
    /// </remarks>
    public class BindableProperty<T>
    {
        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }

        private T mValue = default(T);

        /// <summary>
        /// 当属性值发生变化时，会触发变更事件
        /// </summary>
        public T Value
        {
            get => mValue;

            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;

                mValue = value;
                // 数值发生变化时，通知观察者更新界面（数值驱动）
                mOnValueChanged?.Invoke(mValue);
            }
        }

        //public Action<T> OnValueChanged; // += -= 事件不是很方便
        // 所以使用下面方法

        // 用于存储值变更时的回调方法
        private Action<T> mOnValueChanged = v => { };    // + -

        /// <summary>
        /// 注册值变更事件
        /// </summary>
        /// <param name="onValueChanged">值变更时的回调方法</param>
        /// <returns>返回一个可以用于注销的接口</returns>
        public IUnRegister Register(Action<T> onValueChanged) // +
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyUnregister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            return Register(onValueChanged);
        }

        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }
        // 此方法可以大幅简化比较操作
        // a.Value == b.Value  =>  a == b

        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// 注销值变更事件
        /// </summary>
        /// <param name="onValueChanged">要注销的回调方法</param>
        public void UnRegister(Action<T> onValueChanged)  // -
        {
            mOnValueChanged -= onValueChanged;
        }
    }

    /// <summary>
    /// 可绑定属性的注销类，用于注销值变更事件
    /// </summary>
    /// <typeparam name="T">必须是可比较的类型</typeparam>
    public class BindablePropertyUnregister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        /// <summary>
        /// 注销事件
        /// </summary>
        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);

            // 更好的方式
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
    #endregion
}
