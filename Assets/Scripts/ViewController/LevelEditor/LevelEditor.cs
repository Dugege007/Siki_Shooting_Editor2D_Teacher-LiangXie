using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        public SpriteRenderer EmptyHighLight;

        private int mCurrentHighlightPosX;
        private int mCurrentHighlightPosY;

        private bool mCanDraw;

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // 坐标取整（四舍五入）
            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;

            if (Mathf.Abs(mCurrentHighlightPosX - mouseWorldPos.x) < 0.1f &&
                Mathf.Abs(mCurrentHighlightPosY - mouseWorldPos.y) < 0.1f)
            {

            }
            else
            {
                Vector3 highLightPos = mouseWorldPos;
                highLightPos.z = -9f;
                EmptyHighLight.transform.position = highLightPos;

                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, 20);
                if (hit.collider)
                {
                    EmptyHighLight.color = new Color(1, 0, 0, 0.5f);
                    mCanDraw = false;
                }
                else
                {
                    EmptyHighLight.color = new Color(1, 1, 1, 0.5f);
                    mCanDraw = true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (mCanDraw)
                {
                    GameObject groundPrefab = Resources.Load<GameObject>("Level/Ground");
                    GameObject groundObj = Instantiate(groundPrefab, transform);
                    groundObj.transform.position = mouseWorldPos;
                    groundObj.name = "Ground";
                }
            }
        }
    }
}
