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

            var cameraPos = transform.position;
            var playerPos = mPlayerTrans.position;
            cameraPos.x = playerPos.x + 3;
            cameraPos.y = playerPos.y + 2;

            transform.position = cameraPos;
        }
    }
}
