using UnityEngine;

namespace ShootingEditor2D
{
    public class CameraController : MonoBehaviour
    {
        private Transform mPlayerTrans;

        private void Update()
        {
            if (mPlayerTrans == null)
            {
                var playerGameObj = GameObject.FindWithTag("Player");
                if (playerGameObj != null)
                    mPlayerTrans = playerGameObj.transform;
                else
                    return;
            }

            Vector3 cameraPos = transform.position;
            float isRight = Mathf.Sign(mPlayerTrans.transform.localScale.x);

            Vector3 playerPos = mPlayerTrans.position;
            cameraPos.x = playerPos.x + 3 * isRight;
            cameraPos.y = playerPos.y + 2;

            transform.position = cameraPos;
        }
    }
}
