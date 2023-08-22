using UnityEngine;

namespace ShootingEditor2D
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D mRigidbody2D;
        private Trigger2DCheck mGroundCheck;
        private Gun mGun;

        private bool mJumpPressed;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<Trigger2DCheck>();
            mGun = transform.Find("Gun").GetComponent<Gun>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                mJumpPressed = true;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                mGun.Shoot();
            }
        }

        private void FixedUpdate()
        {
            var horizontalMovement = Input.GetAxis("Horizontal");

            mRigidbody2D.velocity = new Vector2(horizontalMovement * 5f, mRigidbody2D.velocity.y);// y 轴值为 mRigidbody2D.velocity.y 是为了方便之后加跳跃

            var grounded = mGroundCheck.IsTriggered;

            // 当按下按键 && 在地面上时
            if (mJumpPressed && grounded)
            {
                mRigidbody2D.velocity = new Vector2(mRigidbody2D.velocity.x, 5);
            }

            mJumpPressed = false;
        }
    }
}
