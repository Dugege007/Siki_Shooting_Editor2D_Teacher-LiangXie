using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class GunPickItem : MonoBehaviour, IController
    {
        public string Name;
        public int BulletCountInGun;
        public int BulletCountOutGun;

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                this.SendCommand(new PickGunCommand(Name, BulletCountInGun, BulletCountOutGun));

                Destroy(gameObject);
            }
        }
    }
}
