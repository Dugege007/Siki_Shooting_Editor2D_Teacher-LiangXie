using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        public SpriteRenderer EmptyHighLight;

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            mouseWorldPos.z = 0;
            EmptyHighLight.transform.position = mouseWorldPos;
        }
    }
}
