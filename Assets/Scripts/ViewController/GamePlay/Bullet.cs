using FrameworkDesign;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public class Bullet : MonoBehaviour,IController
    {
        private Rigidbody2D mRigidbody2D;

        public IArchitecture GetArchiteccture()
        {
            return ShootingEditor2D.Interface;
        }

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            mRigidbody2D.velocity = Vector2.right * 10;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                this.SendCommand<KillEnemyCommand>();
                Destroy(collision.gameObject);
            }
        }
    }
}
