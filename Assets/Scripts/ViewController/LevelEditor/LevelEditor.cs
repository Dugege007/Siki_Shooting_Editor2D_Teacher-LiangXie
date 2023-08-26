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

            // 坐标取整（四舍五入）
            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;
            EmptyHighLight.transform.position = mouseWorldPos;

            if (Input.GetMouseButtonDown(0))
            {
                GameObject groundPrefab = Resources.Load<GameObject>("Level/Ground");
                GameObject groundObj = Instantiate(groundPrefab, transform);
                groundObj.transform.position = mouseWorldPos;
                groundObj.name = "Ground";
            }
        }
    }
}
