using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class AttackPlayer : MonoBehaviour, IController
    {
        /// <summary>
        /// 造成伤害值
        /// </summary>
        public int Hurt = 1;

        // 获取架构
        // 以访问游戏架构中的其他模块
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
