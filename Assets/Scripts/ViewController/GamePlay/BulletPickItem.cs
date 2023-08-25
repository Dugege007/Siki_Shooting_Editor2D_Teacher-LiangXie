using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingEditor2D
{
    public class BulletPickItem : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }

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
