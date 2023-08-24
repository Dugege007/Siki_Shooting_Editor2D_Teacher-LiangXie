using UnityEngine;

namespace ShootingEditor2D.Test
{
    public class TimeSystemTest : MonoBehaviour
    {
        private void Start()
        {
            // 当前时间
            Debug.Log(Time.time);

            // 获取时间管理系统
            ITimeSystem timeSystem = ShootingEditor2D.Interface.GetSystem<ITimeSystem>();

            // 添加任务
            timeSystem.AddDelayTask(3, () => { Debug.Log(Time.time); });
        }
    }
}
