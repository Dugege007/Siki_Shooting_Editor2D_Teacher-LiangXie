using UnityEngine;

namespace ShootingEditor2D
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D mRigidbody2D;
        private Trigger2DCheck mGroundCheck;
        private Gun mGun;

        // 是否按下跳跃键
        private bool mJumpPressed;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<Trigger2DCheck>();
            mGun = transform.Find("Gun").GetComponent<Gun>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Space))
            {
                mJumpPressed = true;
            }

            if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
            {
                mGun.Shoot();
            }
        }

        private void FixedUpdate()
        {
            var horizontalMovement = Input.GetAxis("Horizontal");

            // 移动转向
            if (horizontalMovement < 0 && transform.localScale.x > 0 || 
                horizontalMovement > 0 && transform.localScale.x < 0)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            }

            // 设置移动速度
            mRigidbody2D.velocity = new Vector2(horizontalMovement * 5f, mRigidbody2D.velocity.y);
            // y 轴值为 mRigidbody2D.velocity.y 是为了方便之后加跳跃

            // 检测是否在地面上
            bool grounded = mGroundCheck.IsTriggered;

            // 当按下按键 && 在地面上时
            if (mJumpPressed && grounded)
            {
                mRigidbody2D.velocity = new Vector2(mRigidbody2D.velocity.x, 5);
            }

            mJumpPressed = false;
        }
    }
}
