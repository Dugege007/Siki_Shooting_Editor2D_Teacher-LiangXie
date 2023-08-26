using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Bullet : ShootingEditor2DController
    {
        private Rigidbody2D mRigidbody2D;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();

            Destroy(gameObject, 5);
        }

        private void Start()
        {
            float isRight = transform.lossyScale.x > 0? 1f : -1f;
            mRigidbody2D.velocity = Vector2.right * 15f * isRight;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                this.SendCommand<KillEnemyCommand>();

                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
