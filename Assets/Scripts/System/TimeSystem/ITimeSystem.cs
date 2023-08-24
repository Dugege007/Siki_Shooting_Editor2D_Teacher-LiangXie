using FrameworkDesign;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingEditor2D
{
    /// <summary>
    /// 时间管理系统 接口
    /// </summary>
    public interface ITimeSystem : ISystem
    {
        float CurrentSeconds { get; }
        void AddDelayTask(float seconds, Action onDelayFinish);
    }

    /// <summary>
    /// 延时任务
    /// </summary>
    public class DelayTask
    {
        /// <summary>
        /// 任务需要执行的秒数
        /// </summary>
        public float Seconds { get; set; }
        /// <summary>
        /// 任务完成时的回调函数
        /// </summary>
        public Action OnFinish { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public float StartSeconds { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public float FinishSeconds { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public DelayTaskState State { get; set; }
    }

    /// <summary>
    /// 延时任务状态 枚举
    /// </summary>
    public enum DelayTaskState
    {
        NotStart,
        Started,
        Finish,
    }

    /// <summary>
    /// 时间管理系统 实现类
    /// </summary>
    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        // 用于触发Unity的Update事件的类
        public class TimeSystemUpdateBehavour : MonoBehaviour
        {
            // 定义一个事件，用于在 Update() 中触发
            public event Action OnUpdate;

            private void Update()
            {
                // 触发 OnUpdate 事件
                OnUpdate?.Invoke();
            }
        }

        // 系统初始化方法
        protected override void OnInit()
        {
            // 创建一个新游戏物体
            GameObject updateBehavourGameObj = new GameObject(nameof(TimeSystemUpdateBehavour));
            // 向该物体添加 TimeSystemUpdateBehavour 组件
            TimeSystemUpdateBehavour updateBehavour = updateBehavourGameObj.AddComponent<TimeSystemUpdateBehavour>();

            // 订阅 OnUpdate 事件
            updateBehavour.OnUpdate += OnUpdate;

            // 初始化当前秒数为 0
            CurrentSeconds = 0;
        }

        /// <summary>
        /// 每帧更新方法
        /// </summary>
        private void OnUpdate()
        {
            // 计时
            CurrentSeconds += Time.deltaTime;

            // 如果有延时任务
            if (mDelayTasks.Count > 0)
            {
                // 获取第一个延时任务
                LinkedListNode<DelayTask> currentNode = mDelayTasks.First;

                // 遍历所有延时任务
                while (currentNode != null)
                {
                    // 获取下一个延时任务
                    LinkedListNode<DelayTask> nextNode = currentNode.Next;
                    // 获取当前延时任务
                    DelayTask delayTask = currentNode.Value;

                    // 根据任务状态处理任务
                    switch (delayTask.State)
                    {
                        case DelayTaskState.NotStart:
                            // 设置任务为已开始
                            delayTask.State = DelayTaskState.Started;
                            // 设置任务开始时间
                            delayTask.StartSeconds = CurrentSeconds;
                            // 设置任务结束时间
                            delayTask.FinishSeconds = CurrentSeconds + delayTask.Seconds;
                            break;

                        case DelayTaskState.Started:
                            // 如果当前时间大于等于结束时间
                            if (CurrentSeconds >= delayTask.FinishSeconds)
                            {
                                // 设置任务为已完成
                                delayTask.State = DelayTaskState.Finish;
                                // 调用任务完成回调
                                delayTask.OnFinish?.Invoke();
                                // 从任务列表中删除任务
                                mDelayTasks.Remove(currentNode);    // 删除当前节点的方式比较节省性能
                            }
                            break;

                        case DelayTaskState.Finish:
                            break;

                        default:
                            break;
                    }

                    // 移动到下一个任务
                    currentNode = nextNode;
                }
            }
        }

        /// <summary>
        /// 当前秒数（只能在内部改变时间）
        /// </summary>
        public float CurrentSeconds { get; private set; }

        // 延时任务链表，方便插入和删除
        private LinkedList<DelayTask> mDelayTasks = new LinkedList<DelayTask>();

        /// <summary>
        /// 添加延时任务
        /// </summary>
        /// <param name="seconds">延时秒数</param>
        /// <param name="onDelayFinish">任务完成回调</param>
        public void AddDelayTask(float seconds, Action onDelayFinish)
        {
            // 创建新的延时任务
            DelayTask delayTask = new DelayTask()
            {
                Seconds = seconds,
                OnFinish = onDelayFinish,
                State = DelayTaskState.NotStart,
            };

            // 将任务添加到任务链表的末尾
            mDelayTasks.AddLast(delayTask);
        }
    }
}
