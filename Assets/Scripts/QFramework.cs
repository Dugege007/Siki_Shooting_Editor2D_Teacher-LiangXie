using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace QFramework
{
    #region Architeture
    /// <summary>
    /// �ܹ��ӿ�
    /// </summary>
    public interface IArchitecture
    {
        /// <summary>
        /// ע��ϵͳ���
        /// </summary>
        /// <typeparam name="T">ϵͳ����</typeparam>
        /// <param name="system">ϵͳʵ��</param>
        void RegisterSystem<T>(T system) where T : ISystem;

        /// <summary>
        /// ע��ģ�����
        /// </summary>
        /// <typeparam name="T">ģ������</typeparam>
        /// <param name="model">ģ��ʵ��</param>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// ע�Ṥ�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="utility">����ʵ��</param>
        void RegisterUtility<T>(T utility) where T : IUtility;

        /// <summary>
        /// ��ȡϵͳ���
        /// </summary>
        /// <typeparam name="T">ϵͳ����</typeparam>
        /// <returns>ϵͳʵ��</returns>
        T GetSystem<T>() where T : class, ISystem;

        /// <summary>
        /// ��ȡģ�����
        /// </summary>
        /// <typeparam name="T">ģ������</typeparam>
        /// <returns>ģ��ʵ��</returns>
        T GetModel<T>() where T : class, IModel;

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <returns>����ʵ��</returns>
        T GetUtility<T>() where T : class, IUtility;

        // ������ܹ������ģʽ�У���������ͨ����������һ�������ġ������õĴ��뵥Ԫ
        // �����һ�ַ�װ���ض����ܻ���Ϊ�Ľṹ�������������Эͬ��������ͬ����һ��������ϵͳ
        // �� Unity ��Ϸ������������Ϸ�����ϵ���������

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        void SendCommand<T>() where T : ICommand, new();

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="command">����ʵ��</param>
        void SendCommand<T>(T command) where T : ICommand;

        /// <summary>
        /// ���Ͳ�ѯ
        /// </summary>
        /// <typeparam name="TResult">��ѯ���صĽ������</typeparam>
        /// <param name="query">��ѯʵ����ʵ����IQuery�ӿڵĶ���</param>
        /// <returns>���ز�ѯ�Ľ��</returns>
        TResult SendQuery<TResult>(IQuery<TResult> query);

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="e">�¼�ʵ��</param>
        void SendEvent<T>(T e);

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="onEvent">�¼�������</param>
        /// <returns>����ע���ӿ�</returns>
        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="onEvent">�¼�������</param>
        void UnRegisterEvent<T>(Action<T> onEvent);
    }

    /// <summary>
    /// �ܹ���
    /// </summary>
    /// <typeparam name="T">�ܹ�����</typeparam>
    /// <remarks>
    /// ������ʹ�� IOC ����ʱҪ�ظ�����Ĵ��롱�ķ��ͻ��ࡣ
    /// </remarks>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()  // �� Singleton ���͵���д��һ��
    {
        // �Ƿ��ʼ�����
        private bool mInited = false;
        // ������ҪҪ��ʼ���� Model���� MakeSureArchitecture() ��ͳһ��ʼ��
        private List<IModel> mModels = new List<IModel>();
        // ������Ҫ��ʼ����System
        private List<ISystem> mSystems = new List<ISystem>();
        // IOC����
        private IOCContainer mContainer = new IOCContainer();

        /// <summary>
        /// ע�Ჹ������
        /// </summary>
        public static Action<T> OnRegisterPatch = architecture => { };

        // �����ܹ�ʵ��
        private static T mArchitecture;
        // �¼�ϵͳ
        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        /// <summary>
        /// ��ȡ�ܹ��ӿ�
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
        /// ȷ���������
        /// </summary>
        private static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                OnRegisterPatch?.Invoke(mArchitecture);

                // �ѻ����ģ�Ͷ���ʼ��
                //ģ�Ͳ��ϵͳ���ӵײ㣬����ϵͳ����ֱ�ӷ���ģ�Ͳ�
                // ����Ӧȷ��ģ�Ͳ��ȳ�ʼ��
                foreach (var model in mArchitecture.mModels)
                {
                    model.Init();
                }

                mArchitecture.mModels.Clear();

                //ϵͳ���ʼ��ʱ���ܻ����ģ�Ͳ�
                foreach (var system in mArchitecture.mSystems)
                {
                    system.Init();
                }

                mArchitecture.mSystems.Clear();

                mArchitecture.mInited = true;
            }
        }

        /// <summary>
        /// �����ʼ������
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// ע��ģ�����
        /// </summary>
        /// <typeparam name="TModel">ģ������</typeparam>
        /// <param name="modelInstance">ģ��ʵ��</param>
        /// <remarks>
        /// ģ�����������ܹ���ͨ��������ʾ���������ݺ�ҵ���߼���״̬��
        /// ��Ҫ���ڣ����ݷ�װ��ҵ���߼���״̬����֪ͨ���������������Ľ��
        /// </remarks>
        public void RegisterModel<TModel>(TModel modelInstance) where TModel : IModel
        {
            // ���üܹ�
            modelInstance.SetArchitecture(this);
            // ע�ᵽ IOC ����
            mContainer.Register<TModel>(modelInstance);

            // ����ܹ���δ��ʼ������Model�����ӵ������б��У��Ժ�ͳһ��ʼ��
            if (!mInited)
                mModels.Add(modelInstance);
            // ����ܹ��ѳ�ʼ����ֱ�ӳ�ʼ��Model���
            else
                modelInstance.Init();
        }

        /// <summary>
        /// ע�Ṥ�����
        /// </summary>
        /// <typeparam name="TUtility">��������</typeparam>
        /// <param name="utilityInstance">����ʵ��</param>
        /// <remarks>
        /// ��������������ṩ����ʵ�ù��ߵ������������־�����ö�ȡ�ȡ�
        /// </remarks>
        public void RegisterUtility<TUtility>(TUtility utilityInstance) where TUtility : IUtility
        {
            // ע�ᵽ IOC ����
            mContainer.Register<TUtility>(utilityInstance);
        }

        /// <summary>
        /// ע��ϵͳ���
        /// </summary>
        /// <typeparam name="TSystem">ϵͳ����</typeparam>
        /// <param name="systemInstance">ϵͳʵ��</param>
        /// <remarks>
        /// ϵͳ���ͨ��������ҵ���߼���Э�����������
        /// </remarks>
        public void RegisterSystem<TSystem>(TSystem systemInstance) where TSystem : ISystem
        {
            // ���üܹ�
            systemInstance.SetArchitecture(this);
            // ע�ᵽIOC����
            mContainer.Register<TSystem>(systemInstance);

            // ����ܹ���δ��ʼ������System�����ӵ������б��У��Ժ�ͳһ��ʼ��
            if (!mInited)
                mSystems.Add(systemInstance);
            // ����ܹ��ѳ�ʼ����ֱ�ӳ�ʼ��System���
            else
                systemInstance.Init();
        }

        /// <summary>
        /// ��ȡģ�����
        /// </summary>
        /// <typeparam name="TModel">ģ�����ͣ�����ʵ�� Iģ�ͽӿ�</typeparam>
        /// <returns>����ָ�����͵�ģ��ʵ��</returns>
        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            // �� IOC �����л�ȡָ�����͵�ģ�����ʵ��
            return mContainer.Get<TModel>();
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <typeparam name="TUtility">�������ͣ�����ʵ�� I���߽ӿ�</typeparam>
        /// <returns>����ָ�����͵Ĺ���ʵ��</returns>
        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            // �� IOC �����л�ȡָ�����͵Ĺ������ʵ��
            return mContainer.Get<TUtility>();
        }

        /// <summary>
        /// ��ȡϵͳ���
        /// </summary>
        /// <typeparam name="TSystem">ϵͳ���ͣ�����ʵ�� Iϵͳ�ӿ�</typeparam>
        /// <returns>����ָ�����͵�ϵͳʵ��</returns>
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            // �� IOC �����л�ȡָ�����͵�ϵͳ���ʵ��
            return mContainer.Get<TSystem>();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TCommand">�������ͣ�����ʵ�� ICommand �ӿڣ��������޲����Ĺ��캯��</typeparam>
        /// <remarks>
        /// ����ģʽ���ڷ�װ������Ϊ���󣬴Ӷ������û������������Ŷ����󡢲��ṩ�������ܡ�
        /// </remarks>
        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            // ����ָ�����͵�����ʵ��
            var command = new TCommand();
            // ������ܹ�Ϊ��ǰ�ܹ�
            command.SetArchitecture(this);
            // ִ������
            command.Execute();
            // ִ��������������ܹ�֮������ã����ⲻ��Ҫ�����ó���
            command.SetArchitecture(null);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TCommand">�������ͣ�����ʵ�� ICommand �ӿ�</typeparam>
        /// <param name="command">Ҫִ�е�����ʵ��</param>
        /// <remarks>
        /// ����ģʽ���ڷ�װ������Ϊ���󣬴Ӷ������û������������Ŷ����󡢲��ṩ�������ܡ�
        /// </remarks>
        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            // ������ܹ�Ϊ��ǰ�ܹ�
            command.SetArchitecture(this);
            // ִ������
            command.Execute();
        }

        /// <summary>
        /// ���Ͳ�ѯ����
        /// </summary>
        /// <typeparam name="TResult">��ѯ���������</typeparam>
        /// <param name="query">ʵ���� IQuery �ӿڵĲ�ѯ����</param>
        /// <returns>���ز�ѯ�Ľ��</returns>
        /// <remarks>
        /// �÷���������һ����ѯ���󣬲���ȡ��ѯ�Ľ����
        /// �������ڸ÷�������Ӹ����ѯ����
        /// </remarks>
        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <typeparam name="TEvent">�¼����ͣ���������޲����Ĺ��캯��</typeparam>
        /// <remarks>
        /// �¼�ϵͳ�������֮���������ϵ�ͨ�ţ�ʹ���¼��ķ����ߺͽ����߲���ֱ���໥���á�
        /// </remarks>
        public void SendEvent<TEvent>() where TEvent : new()
        {
            // ����ָ�����͵��¼�
            mTypeEventSystem.Send<TEvent>();
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <typeparam name="TEvent">�¼�����</typeparam>
        /// <param name="e">�¼�ʵ��</param>
        /// <remarks>
        /// �¼�ϵͳ�������֮���������ϵ�ͨ�ţ�ʹ���¼��ķ����ߺͽ����߲���ֱ���໥���á�
        /// </remarks>
        public void SendEvent<TEvent>(TEvent e)
        {
            // ����ָ�����͵��¼�
            mTypeEventSystem.Send<TEvent>(e);
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <typeparam name="TEvent">�¼�����</typeparam>
        /// <param name="onEvent">�¼������ί�з���</param>
        /// <returns>����һ��ע���ӿڣ�������ע�����¼�</returns>
        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <typeparam name="TEvent">�¼�����</typeparam>
        /// <param name="onEvent">Ҫע�����¼������ί�з���</param>
        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }
    }
    #endregion

    #region Controller
    /// <summary>
    /// ���ֲ�������ӿ�
    /// </summary>
    /// <remarks>
    /// �ýӿڶ����˱��ֲ�������Ļ������ܺ���Ϊ����������ܹ����ɻ�ȡϵͳ���ɻ�ȡģ�͡��ɷ��������ע���¼����ɷ��Ͳ�ѯ��
    /// ���ڱ��ֲ���󾭳����д��������٣���˽����ֲ����ע�ᵽ�ܹ���û�����壻
    /// ͨ��ʵ�ִ˽ӿڣ����Ա�Ǳ��ֲ���󣬲�ʹ���ܹ����ʼܹ��е�ϵͳ��ģ�ͣ�������ʹ�õ�����ʽ��ȡ��
    /// </remarks>
    public interface IController : IBelongToArchitecture, ICanGetSystem, ICanGetModel, ICanSendCommand, ICanRegisterEvent, ICanSendQuery
    {
        // �˽ӿ���Ҫ��������������ܣ�����Ҫ����ķ�������
    }
    #endregion

    #region System
    /// <summary>
    /// ϵͳ��ӿ�
    /// </summary>
    /// <remarks>
    /// ������ϵͳ��Ļ������ܺ���Ϊ����������ܹ��������üܹ����ɻ�ȡϵͳ���ɻ�ȡģ�͡��ɻ�ȡ���ߡ���ע���¼����ɷ����¼���
    /// </remarks>
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent
    {
        /// <summary>
        /// ��ʼ��ϵͳ
        /// </summary>
        void Init();
    }

    /// <summary>
    /// ����ϵͳ��
    /// </summary>
    /// <remarks>
    /// ʵ��ϵͳ��ӿڵĻ������ܡ�
    /// </remarks>
    public abstract class AbstractSystem : ISystem
    {
        // ����ܹ�����
        private IArchitecture mArchitecture;

        /// <summary>
        /// ��ȡ�ܹ�
        /// </summary>
        /// <returns>���ص�ǰϵͳ�ļܹ�</returns>
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        // ��ʽ�ӿ�ʵ�֣�
        // ��װ������ʹ��ֻ��ͨ���ӿ������ʣ�������ͨ�����ʵ��ֱ�ӷ��ʡ�������������ʵ��ϸ�ڣ�ʹ������ӷ�װ��
        // ���������ͻ�����һ����ʵ���˶���ӿڣ�����Щ�ӿ�������ͬ�ķ���ǩ������ô��ʽ�ӿ�ʵ�ֿ����������������⡣����Ϊÿ���ӿ��ṩ��ͬ��ʵ�֣������������ͻ��
        // �����Ĵ���ṹ����ʽ�ӿ�ʵ�ֻ�����ʹ����ṹ�������������������Щ������Ϊ��ʵ���ĸ��ӿڡ�

        /// <summary>
        /// ��ʼ��ϵͳ
        /// </summary>
        /// <remarks>
        /// �ɾ��������ʵ�֡�
        /// </remarks>
        void ISystem.Init()
        {
            OnInit();
        }

        /// <summary>
        /// ���üܹ�
        /// </summary>
        /// <param name="architecture">Ҫ���õļܹ�����</param>
        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <remarks>
        /// �ɾ��������ʵ�֡�
        /// </remarks>
        protected abstract void OnInit();
    }
    #endregion

    #region Model
    /// <summary>
    /// ģ�Ͳ�ӿ�
    /// </summary>
    /// <remarks>
    /// ������ģ�Ͳ�Ļ������ܺ���Ϊ����������ܹ��������üܹ����ɻ�ȡ���ߡ��ɷ����¼���
    /// </remarks>
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        /// <summary>
        /// ��ʼ��ģ��
        /// </summary>
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        // ����ܹ�����
        private IArchitecture mArchitecture;

        /// <summary>
        /// ��ȡ�ܹ�
        /// </summary>
        /// <returns>���ص�ǰģ�͵ļܹ�</returns>
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        /// <summary>
        /// ��ʼ��ģ��
        /// </summary>
        /// <remarks>
        /// �ɾ��������ʵ�֡�
        /// </remarks>
        void IModel.Init()
        {
            OnInit();
        }

        /// <summary>
        /// ���üܹ�
        /// </summary>
        /// <param name="architecture">Ҫ���õļܹ�����</param>
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <remarks>
        /// �ɾ��������ʵ�֡�
        /// </remarks>
        protected abstract void OnInit();
    }
    #endregion

    #region Utility
    /// <summary>
    /// ���߲�ӿ�
    /// </summary>
    public interface IUtility
    {
        // �˽ӿڲ���Ҫ����ķ�������
    }
    #endregion

    #region Command
    /// <summary>
    /// ����ӿ�
    /// </summary>
    /// <remarks>
    /// ���ڴ������߼����ֵ��������Ľ����߼�ְ��
    /// �������ܺ���Ϊ����������ܹ��������üܹ����ɻ�ȡϵͳ���ɻ�ȡģ�͡��ɻ�ȡ���ߡ��ɷ�������ɷ����¼����ɷ��Ͳ�ѯ��
    /// ͨ�������ҵĽ����߼�����ӿ�����Ǩ�Ƶ������У�ʹ����ṹ��������
    /// </remarks>
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        /// <summary>
        /// ִ������
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <remarks>
    /// ʵ������ӿڵĻ������ܣ��ṩִ������Ļ����ṹ��
    /// </remarks>
    public abstract class AbstractCommand : ICommand
    {
        // ����ܹ�����
        private IArchitecture mArchitecture;

        /// <summary>
        /// ִ������
        /// </summary>
        /// <remarks>
        /// �ɾ��������ʵ�֡�
        /// </remarks>
        void ICommand.Execute()
        {
            OnExecute();
        }

        /// <summary>
        /// ��ȡ�ܹ�
        /// </summary>
        /// <returns>���ص�ǰ����ļܹ�</returns>
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        /// <summary>
        /// ���üܹ�
        /// </summary>
        /// <param name="architecture">Ҫ���õļܹ�����</param>
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        /// <summary>
        /// ִ������
        /// </summary>
        /// <remarks>
        /// �ɾ��������ʵ�֡�
        /// </remarks>
        protected virtual void OnExecute() { }
    }
    #endregion

    #region Query
    /// <summary>
    /// ��ѯ �ӿ�
    /// </summary>
    /// <typeparam name="TResult">��ѯ���������</typeparam>
    /// <remarks>
    /// �����˲�ѯ�Ļ������ܺ���Ϊ����������ܹ��������üܹ����ɻ�ȡϵͳ���ɻ�ȡģ�͡��ɷ��Ͳ�ѯ��
    /// ���ڻ�ȡ�ض��Ľ����
    /// </remarks>
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanSendQuery
    {
        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <returns>���ز�ѯ�Ľ��</returns>
        TResult Do();
    }

    /// <summary>
    /// ���� ��ѯ��
    /// </summary>
    /// <typeparam name="T">��ѯ���������</typeparam>
    /// <remarks>
    /// ʵ�ֲ�ѯ�ӿڵĻ������ܡ�
    /// </remarks>
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        // ����ܹ��������ڷ�������ܹ��Ĺ���
        private IArchitecture mArchitecture;

        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <returns>���ز�ѯ�Ľ��</returns>
        public T Do()
        {
            return OnDo();
        }

        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <returns>���ز�ѯ�Ľ��</returns>
        /// <remarks>
        /// ��������ʵ�־���Ĳ�ѯ�߼���
        /// </remarks>
        protected abstract T OnDo();    // �������д�����

        /// <summary>
        /// ��ȡ�ܹ�
        /// </summary>
        /// <returns>���ص�ǰ��ѯ�����ļܹ��ӿ�</returns>
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        /// <summary>
        /// ���üܹ�
        /// </summary>
        /// <param name="architecture">Ҫ���õļܹ�����</param>
        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
    #endregion

    #region Rule
    /// <summary>
    /// ��������ĸ��ܹ� �ӿ�
    /// </summary>
    /// <remarks>
    /// ���� Architecture �е� System �� Model ���໥���ã�����������Ҫһ���ӿ��������� System ���� Model �������ĸ� Architecture �ģ�
    /// ��� Architecture �еĵݹ���á�
    /// </remarks>
    public interface IBelongToArchitecture
    {
        /// <summary>
        /// ��ȡ�����ļܹ�
        /// </summary>
        /// <returns>���������ļܹ�ʵ��</returns>
        IArchitecture GetArchitecture();
    }

    /// <summary>
    /// �����üܹ� �ӿ�
    /// </summary>
    /// <remarks>
    /// ���ܹ�ʵ�����õ�ʵ�ִ˽ӿڵ����У�����ȷ��������������ļܹ�֮�����ȷ������
    /// </remarks>
    public interface ICanSetArchitecture
    {
        /// <summary>
        /// ���������ļܹ�
        /// </summary>
        /// <param name="architecture">�ܹ�ʵ��</param>
        void SetArchitecture(IArchitecture architecture);
    }

    /// <summary>
    /// �ɻ�ȡϵͳ �ӿ�
    /// </summary>
    public interface ICanGetSystem : IBelongToArchitecture { }

    public static class CanGetSystemExtension
    {
        /// <summary>
        /// ��չ�������Ӽܹ��л�ȡϵͳ
        /// </summary>
        /// <typeparam name="T">ϵͳ���ͣ�����ʵ�� ISystem �ӿ�</typeparam>
        /// <param name="self">ʵ�� ICanGetSystem �ӿڵ�ʵ��</param>
        /// <returns>����ָ�����͵�ϵͳʵ��</returns>
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }

    /// <summary>
    /// �ɻ�ȡģ�� �ӿ�
    /// </summary>
    public interface ICanGetModel : IBelongToArchitecture { }

    public static class CanGetModelExtension
    {
        /// <summary>
        /// ��չ�������Ӽܹ��л�ȡģ��
        /// </summary>
        /// <typeparam name="T">ģ�����ͣ�����ʵ�� IModel �ӿ�</typeparam>
        /// <param name="self">ʵ�� ICanGetModel �ӿڵ�ʵ��</param>
        /// <returns>����ָ�����͵�ģ��ʵ��</returns>
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }

    /// <summary>
    /// ��ע�ᡢע���¼� �ӿ�
    /// </summary>
    /// <remarks>
    /// �¼���һ����Ϣ���ƣ������ڲ�ͬ���֮�䴫����Ϣ��
    /// </remarks>
    public interface ICanRegisterEvent : IBelongToArchitecture { }

    public static class CanRegisterEventExtension
    {
        /// <summary>
        /// ��չ������ע���¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="self">ʵ�� ICanRegisterEvent �ӿڵ�ʵ��</param>
        /// <param name="onEvent">�¼�����Ļص�����</param>
        /// <returns>����һ�� IUnRegister �ӿڣ���������֮���ע������</returns>
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }

        /// <summary>
        /// ��չ������ע���¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="self">ʵ�� ICanRegisterEvent �ӿڵ�ʵ��</param>
        /// <param name="onEvent">�¼�����Ļص�����</param>
        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }

    /// <summary>
    /// �ɷ����¼� �ӿ�
    /// </summary>
    /// <remarks>
    /// �¼���һ����Ϣ���ƣ������ڲ�ͬ���֮�䴫����Ϣ��
    /// </remarks>
    public interface ICanSendEvent : IBelongToArchitecture { }

    public static class CanSendEventExtension
    {
        /// <summary>
        /// ��չ�����������¼�
        /// </summary>
        /// <typeparam name="T">�¼����ͣ���������޲����Ĺ��캯��</typeparam>
        /// <param name="self">ʵ�� ICanSendEvent �ӿڵ�ʵ��</param>
        /// <remarks>
        /// �˷�������һ���µ��¼�ʵ�������͡��¼��ľ����߼��������ܹ�����
        /// </remarks>
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        /// <summary>
        /// ���ŷ����������¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="self">ʵ�� ICanSendEvent �ӿڵ�ʵ��</param>
        /// <param name="e">�¼�ʵ��</param>
        /// <remarks>
        /// �˷�������һ���Ѵ��ڵ��¼�ʵ�����¼��ľ����߼��������ܹ�����
        /// </remarks>
        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }

    /// <summary>
    /// �ɷ������� �ӿ�
    /// </summary>
    /// <remarks>
    /// ����ģʽ���������װΪ���󣬴Ӷ�ʹ�����ߺͽ�����֮����
    /// </remarks>
    public interface ICanSendCommand : IBelongToArchitecture { }

    public static class CanSendCommandExtension
    {
        /// <summary>
        /// ��չ��������������
        /// </summary>
        /// <typeparam name="T">�������ͣ�����ʵ�� ICommand �ӿڲ������޲����Ĺ��캯��</typeparam>
        /// <param name="self">ʵ�� ICanSendCommand �ӿڵ�ʵ��</param>
        /// <remarks>
        /// �˷�������һ���µ�����ʵ�������͡�����ľ����߼��������ܹ�����
        /// </remarks>
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        /// <summary>
        /// ��չ��������������
        /// </summary>
        /// <typeparam name="T">�������ͣ�����ʵ�� ICommand �ӿ�</typeparam>
        /// <param name="self">ʵ�� ICanSendCommand �ӿڵ�ʵ��</param>
        /// <param name="command">����ʵ��</param>
        /// <remarks>
        /// �˷�������һ���Ѵ��ڵ�����ʵ��������ľ����߼��������ܹ�����
        /// </remarks>
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }

    /// <summary>
    /// �ɻ�ȡ���� �ӿ�
    /// </summary>
    public interface ICanGetUtility : IBelongToArchitecture { }

    public static class CanGetUtilityExtension
    {
        /// <summary>
        /// ��չ�������Ӽܹ��л�ȡ����
        /// </summary>
        /// <typeparam name="T">�������ͣ�����ʵ�� IUtility �ӿ�</typeparam>
        /// <param name="self">ʵ�� ICanGetUtility �ӿڵ�ʵ��</param>
        /// <returns>����ָ�����͵Ĺ���ʵ��</returns>
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
    /// �������͵��¼����� �ӿ�
    /// </summary>
    /// <remarks>
    /// �����˷��͡�ע���ע���¼��ķ�����
    /// </remarks>
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }

    // ����ע���¼�������������ʱҪע�����󣬲���ȽϷ���
    // Ϊ�˼򻯲���������һ�� IUnRegister �ӿ�

    /// <summary>
    /// ע���¼� �ӿ�
    /// </summary>
    /// <remarks>
    /// �ṩע���¼��ķ�����
    /// </remarks>
    public interface IUnRegister
    {
        void UnRegister();
    }

    // ʹ�ýṹ����һЩ...

    /// <summary>
    /// ע���¼� �ṹ��
    /// </summary>
    /// <typeparam name="T">�¼�����</typeparam>
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
    /// ע���¼��Ĵ�����
    /// </summary>
    /// <remarks>
    /// �����ص���Ϸ��������ʱ�����Զ�ע������ע����¼���
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

    // Ϊ�˼�ʹ��...
    public static class UnRegisterExtension
    {
        /// <summary>
        /// ��չ������Ϊ������ʱ��Ҫע�����������¼������壬���ע���¼��Ĵ�����
        /// </summary>
        /// <param name="unRegister">Ҫע�����¼�</param>
        /// <param name="obj">��Ϸ����</param>
        /// <remarks>
        /// ���ڼ�ע���¼��Ĳ�����
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
    /// �������͵��¼�����ʵ����
    /// </summary>
    /// <remarks>
    /// �ṩ��ע�ᡢ���ͺ�ע���¼��Ĺ��ܡ�
    /// </remarks>
    public class TypeEventSystem : ITypeEventSystem
    {
        // �¼����Ʊ������һ�����ݽṹ�������ֵ�

        // ����һ���ӿڣ�����ע���¼�
        // ͨ�� Register �������һ��ע�ᣬ����ӿڻ��ж��ע��
        // һ����Ϣ�Ĺؼ��ֻ��ж��ע��
        public interface IRegistrations { }

        /// <summary>
        /// ע���ض����͵��¼� ������
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }

        // ʹ���ֵ�洢����ע����¼�
        Dictionary<Type, IRegistrations> mEventRegistration = new Dictionary<Type, IRegistrations>();

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="onEvent">�¼�������</param>
        /// <returns>����һ����������ע���Ľӿ�</returns>
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
        /// �����¼�
        /// </summary>
        /// <typeparam name="T">�¼����ͣ��������޲ι��캯��</typeparam>
        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="e">�¼�ʵ��</param>
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
        /// ע������
        /// </summary>
        /// <typeparam name="T">�¼�����</typeparam>
        /// <param name="onEvent">Ҫע�����¼�����</param>
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
    /// IOC���� (Inversion of Control ���Ʒ�ת)
    /// Ҳ������ע�루Dependency Injection��DI��
    /// </summary>
    /// <remarks>
    /// IOC�������ڹ������Ĵ������������ڣ�����������ϵ�Ӵ����н��
    /// ͨ��������������ע��ͼ����������󣬴Ӷ�ʵ�ֿ�����Ĺ�������á�
    /// </remarks>
    public class IOCContainer
    {
        // �洢��ע��ĵ�������
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        /// <summary>
        /// ע�ᵥ������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="instance">��������ʵ��</param>
        /// <remarks>
        /// �����������ע�ᣬ����������ʵ����
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
        /// ��ȡ��ע��ĵ�������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <returns>���ص�������ʵ�������δע���򷵻� null</returns>
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
    /// ���͵���������
    /// </summary>
    /// <typeparam name="T">�����������</typeparam>
    /// <remarks>
    /// �����ṩ��һ��ͨ�õĵ���ģʽʵ�֡�
    /// ͨ�����䣬�������Զ������͹����κξ��зǹ���Ĭ�Ϲ��캯������ĵ���ʵ����
    /// </remarks>
    public class Singleton<T> where T : Singleton<T>
    {
        // �洢����ʵ��
        private static T mInstance;

        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <value>����ʵ��</value>
        /// <exception cref="Exception">����Ҳ����ǹ�����Ĭ�Ϲ��캯�������׳��쳣</exception>
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var type = typeof(T);

                    // ���÷��䣬��ȡ T �Ĺ��캯������
                    // BindingFlags �󶨱�ǣ�Instance ʵ����NonPublic ��public��
                    var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

                    // ��ȡ��һ���޲ι��죨Ĭ�Ϲ��죩
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                    if (ctor == null)
                    {
                        // �׳��쳣
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
    // BindableProperty �ɰ󶨵����Եļ�ʵ��
    // ���ڿɱȽϵ�ֵ�ĸ���
    // ���� + ���ݱ���¼� �ĺ��壬�ȴ洢���ݣ��ֳ䵱C#�е����Խ�ɫ��Ҳ�����ñ�ĵط����������ݵı���¼����������������������룻
    // ���ټӸ���ҹ��ܾ�Ҫдһ��һģһ���Ĵ��룬��������һ�� mGold��OnGoldChanged ί�С�Gold ����ֵ�Ƚ��߼�������������룻
    // ���Ժܶ��ܶ����ô������ɻ��߷��ͷ�ʽȥ�����������ı�дʱ�䣻
    // �Ե����ϣ����ӽڵ�֪ͨ���ڵ㣩���߼���ϵ����ʹ��ί�л��¼����Զ����£��縸�ڵ�����ӽڵ㣩���߼���ϵ����ʹ�÷�������

    /// <summary>
    /// �ɰ����Եļ�ʵ�֣����ڿɱȽϵ�ֵ�ĸ���
    /// </summary>
    /// <typeparam name="T">�Ƚϵ�����</typeparam>
    /// <remarks>
    /// ���� + ���ݱ���¼��ĺ��壬�ȴ洢���ݣ��ֳ䵱C#�е����Խ�ɫ��Ҳ�����ñ�ĵط����������ݵı���¼���
    /// </remarks>
    public class BindableProperty<T>
    {
        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }

        private T mValue = default(T);

        /// <summary>
        /// ������ֵ�����仯ʱ���ᴥ������¼�
        /// </summary>
        public T Value
        {
            get => mValue;

            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;

                mValue = value;
                // ��ֵ�����仯ʱ��֪ͨ�۲��߸��½��棨��ֵ������
                mOnValueChanged?.Invoke(mValue);
            }
        }

        //public Action<T> OnValueChanged; // += -= �¼����Ǻܷ���
        // ����ʹ�����淽��

        // ���ڴ洢ֵ���ʱ�Ļص�����
        private Action<T> mOnValueChanged = v => { };    // + -

        /// <summary>
        /// ע��ֵ����¼�
        /// </summary>
        /// <param name="onValueChanged">ֵ���ʱ�Ļص�����</param>
        /// <returns>����һ����������ע���Ľӿ�</returns>
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
        // �˷������Դ���򻯱Ƚϲ���
        // a.Value == b.Value  =>  a == b

        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// ע��ֵ����¼�
        /// </summary>
        /// <param name="onValueChanged">Ҫע���Ļص�����</param>
        public void UnRegister(Action<T> onValueChanged)  // -
        {
            mOnValueChanged -= onValueChanged;
        }
    }

    /// <summary>
    /// �ɰ����Ե�ע���࣬����ע��ֵ����¼�
    /// </summary>
    /// <typeparam name="T">�����ǿɱȽϵ�����</typeparam>
    public class BindablePropertyUnregister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        /// <summary>
        /// ע���¼�
        /// </summary>
        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);

            // ���õķ�ʽ
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
    #endregion
}
