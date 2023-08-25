using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class SupplyStation : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                this.SendCommand<FullBulletCommand>();
            }
        }
    }
}
