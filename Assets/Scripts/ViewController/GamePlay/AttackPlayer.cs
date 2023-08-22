using FrameworkDesign;
using UnityEngine;

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
