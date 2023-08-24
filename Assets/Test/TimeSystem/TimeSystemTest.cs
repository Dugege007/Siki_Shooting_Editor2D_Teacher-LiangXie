using UnityEngine;

namespace ShootingEditor2D.Test
{
    public class TimeSystemTest : MonoBehaviour
    {
        private void Start()
        {
            // ��ǰʱ��
            Debug.Log(Time.time);

            // ��ȡʱ�����ϵͳ
            ITimeSystem timeSystem = ShootingEditor2D.Interface.GetSystem<ITimeSystem>();

            // �������
            timeSystem.AddDelayTask(3, () => { Debug.Log(Time.time); });
        }
    }
}
