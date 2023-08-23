using UnityEngine;

namespace ShootingEditor2D
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D mRigidbody2D;

        private Trigger2DCheck mWallCheck;
        private Trigger2DCheck mFallCheck;
        private Trigger2DCheck mGroundCheck;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();

            mWallCheck = transform.Find("WallCheck").GetComponent<Trigger2DCheck>();
            mFallCheck = transform.Find("FallCheck").GetComponent<Trigger2DCheck>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<Trigger2DCheck>();
        }

        private void FixedUpdate()
        {
            float scaleX = transform.localScale.x;

            // 如果在地面上、前方还有路、前方没有墙
            if (mGroundCheck.IsTriggered && mFallCheck.IsTriggered && !mWallCheck.IsTriggered)
            {
                // 设置移动速度
                mRigidbody2D.velocity = new Vector2(scaleX * 3f, mRigidbody2D.velocity.y);
            }
            else
            {
                // 转向
                Vector3 localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            }
        }
    }
}
