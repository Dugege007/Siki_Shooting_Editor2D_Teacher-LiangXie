using FrameworkDesign;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：攻击玩家命令
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public class AttackPlayer : MonoBehaviour,IController
    {
        public int Hurt = 1;

        public IArchitecture GetArchiteccture()
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
