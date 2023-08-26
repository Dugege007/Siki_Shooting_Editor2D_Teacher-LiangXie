using FrameworkDesign;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingEditor2D
{
    /// <summary>
    /// ʱ�����ϵͳ �ӿ�
    /// </summary>
    public interface ITimeSystem : ISystem
    {
        float CurrentSeconds { get; }
        void AddDelayTask(float seconds, Action onDelayFinish);
    }

    /// <summary>
    /// ��ʱ����
    /// </summary>
    public class DelayTask
    {
        /// <summary>
        /// ������Ҫִ�е�����
        /// </summary>
        public float Seconds { get; set; }
        /// <summary>
        /// �������ʱ�Ļص�����
        /// </summary>
        public Action OnFinish { get; set; }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public float StartSeconds { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public float FinishSeconds { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        public DelayTaskState State { get; set; }
    }

    /// <summary>
    /// ��ʱ����״̬ ö��
    /// </summary>
    public enum DelayTaskState
    {
        NotStart,
        Started,
        Finish,
    }

    /// <summary>
    /// ʱ�����ϵͳ ʵ����
    /// </summary>
    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        // ���ڴ���Unity��Update�¼�����
        public class TimeSystemUpdateBehavour : MonoBehaviour
        {
            // ����һ���¼��������� Update() �д���
            public event Action OnUpdate;

            private void Update()
            {
                // ���� OnUpdate �¼�
                OnUpdate?.Invoke();
            }
        }

        // ϵͳ��ʼ������
        protected override void OnInit()
        {
            // ����һ������Ϸ����
            GameObject updateBehavourGameObj = new GameObject(nameof(TimeSystemUpdateBehavour));
            // ���������� TimeSystemUpdateBehavour ���
            TimeSystemUpdateBehavour updateBehavour = updateBehavourGameObj.AddComponent<TimeSystemUpdateBehavour>();

            // ���� OnUpdate �¼�
            updateBehavour.OnUpdate += OnUpdate;

            // ��ʼ����ǰ����Ϊ 0
            CurrentSeconds = 0;
        }

        /// <summary>
        /// ÿ֡���·���
        /// </summary>
        private void OnUpdate()
        {
            // ��ʱ
            CurrentSeconds += Time.deltaTime;

            // �������ʱ����
            if (mDelayTasks.Count > 0)
            {
                // ��ȡ��һ����ʱ����
                LinkedListNode<DelayTask> currentNode = mDelayTasks.First;

                // ����������ʱ����
                while (currentNode != null)
                {
                    // ��ȡ��һ����ʱ����
                    LinkedListNode<DelayTask> nextNode = currentNode.Next;
                    // ��ȡ��ǰ��ʱ����
                    DelayTask delayTask = currentNode.Value;

                    // ��������״̬��������
                    switch (delayTask.State)
                    {
                        case DelayTaskState.NotStart:
                            // ��������Ϊ�ѿ�ʼ
                            delayTask.State = DelayTaskState.Started;
                            // ��������ʼʱ��
                            delayTask.StartSeconds = CurrentSeconds;
                            // �����������ʱ��
                            delayTask.FinishSeconds = CurrentSeconds + delayTask.Seconds;
                            break;

                        case DelayTaskState.Started:
                            // �����ǰʱ����ڵ��ڽ���ʱ��
                            if (CurrentSeconds >= delayTask.FinishSeconds)
                            {
                                // ��������Ϊ�����
                                delayTask.State = DelayTaskState.Finish;
                                // ����������ɻص�
                                delayTask.OnFinish?.Invoke();
                                // �������б���ɾ������
                                mDelayTasks.Remove(currentNode);    // ɾ����ǰ�ڵ�ķ�ʽ�ȽϽ�ʡ����
                            }
                            break;

                        case DelayTaskState.Finish:
                            break;

                        default:
                            break;
                    }

                    // �ƶ�����һ������
                    currentNode = nextNode;
                }
            }
        }

        /// <summary>
        /// ��ǰ������ֻ�����ڲ��ı�ʱ�䣩
        /// </summary>
        public float CurrentSeconds { get; private set; }

        // ��ʱ����������������ɾ��
        private LinkedList<DelayTask> mDelayTasks = new LinkedList<DelayTask>();

        /// <summary>
        /// �����ʱ����
        /// </summary>
        /// <param name="seconds">��ʱ����</param>
        /// <param name="onDelayFinish">������ɻص�</param>
        public void AddDelayTask(float seconds, Action onDelayFinish)
        {
            // �����µ���ʱ����
            DelayTask delayTask = new DelayTask()
            {
                Seconds = seconds,
                OnFinish = onDelayFinish,
                State = DelayTaskState.NotStart,
            };

            // ��������ӵ����������ĩβ
            mDelayTasks.AddLast(delayTask);
        }
    }
}
