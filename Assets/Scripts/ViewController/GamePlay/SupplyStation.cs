using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class SupplyStation : ShootingEditor2DController
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                this.SendCommand<FullBulletCommand>();
            }
        }
    }
}
