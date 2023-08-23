using UnityEngine;

namespace ShootingEditor2D
{
    public class CameraController : MonoBehaviour
    {
        private Transform mPlayerTrans;

        private float xMin = -5;
        private float xMax = 5;
        private float yMin = -5;
        private float yMax = 5;

        private Vector3 mTargetPos;

        private void LateUpdate()
        {
            if (mPlayerTrans == null)
            {
                var playerGameObj = GameObject.FindWithTag("Player");
                if (playerGameObj != null)
                    mPlayerTrans = playerGameObj.transform;
                else
                    return;
            }

            float isRight = Mathf.Sign(mPlayerTrans.transform.localScale.x);

            Vector3 playerPos = mPlayerTrans.position;
            mTargetPos.x = playerPos.x + 3 * isRight;
            mTargetPos.y = playerPos.y + 2;
            mTargetPos.z = -10;

            float smoothSpeed = 5f;

            // 增加一个平滑处理（通过插值）
            Vector3 currentPos = transform.position;
            currentPos = Vector3.Lerp(currentPos, mTargetPos, smoothSpeed * Time.deltaTime);

            // 锁定在一个固定区域
            transform.position = new Vector3(Mathf.Clamp(currentPos.x, xMin, xMax), Mathf.Clamp(currentPos.y, yMin, yMax), currentPos.z);

            // 目前还存在角色移动时会抖动的问题
        }
    }
}
