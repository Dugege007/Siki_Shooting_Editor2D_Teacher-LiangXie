using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class BulletPickItem : ShootingEditor2DController
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                this.SendCommand<AddBulletCommand>();

                Destroy(gameObject);
            }
        }
    }
}
