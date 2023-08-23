using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class AttackPlayer : MonoBehaviour, IController
    {
        /// <summary>
        /// ����˺�ֵ
        /// </summary>
        public int Hurt = 1;

        // ��ȡ�ܹ�
        // �Է�����Ϸ�ܹ��е�����ģ��
        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                this.SendCommand(new HurtPlayerCommand(Hurt));
            }
        }
    }
}
