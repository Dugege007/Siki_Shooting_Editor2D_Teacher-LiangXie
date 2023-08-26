using System;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        public enum OperateMode
        {
            Draw,
            Erase
        }

        public SpriteRenderer EmptyHighLight;

        private OperateMode mCurrentOperateMode;

        private int mCurrentHighlightPosX;
        private int mCurrentHighlightPosY;
        private bool mCanDraw;

        private GameObject mCurrentObjectMouseOn;

        private Lazy<GUIStyle> mModelLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 50,
            alignment = TextAnchor.MiddleCenter,
        });

        private Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter,
        });

        private void OnGUI()
        {
            Rect modeLabelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, 35f, 300f, 50f);
            GUI.Label(modeLabelRect, mCurrentOperateMode.ToString(), mModelLabelStyle.Value);

            var drawButtonRect = new Rect(20, 20, 200, 60);
            if (GUI.Button(drawButtonRect, "绘制", mButtonStyle.Value))
                mCurrentOperateMode = OperateMode.Draw;

            var eraseButtonRect = new Rect(20, 100, 200, 60);
            if (GUI.Button(eraseButtonRect, "橡皮", mButtonStyle.Value))
                mCurrentOperateMode = OperateMode.Erase;
        }

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // 坐标取整（四舍五入）
            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;

            if (GUIUtility.hotControl == 0)
            {
                EmptyHighLight.gameObject.SetActive(true);
            }
            else
            {
                EmptyHighLight.gameObject.SetActive(false);
            }

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
                    if (mCurrentOperateMode == OperateMode.Draw)
                    {
                        EmptyHighLight.color = new Color(1, 0, 0, 0.5f);    // 红色
                    }
                    else if (mCurrentOperateMode == OperateMode.Erase)
                    {
                        EmptyHighLight.color = new Color(1, 0.5f, 0, 0.5f); // 橙色
                    }
                    mCanDraw = false;

                    mCurrentObjectMouseOn = hit.collider.gameObject;
                }
                else
                {
                    if (mCurrentOperateMode == OperateMode.Draw)
                    {
                        EmptyHighLight.color = new Color(1, 1, 1, 0.5f);    // 白色
                    }
                    else if (mCurrentOperateMode == OperateMode.Erase)
                    {
                        EmptyHighLight.color = new Color(0, 0, 1, 0.5f);    // 蓝色
                    }
                    mCanDraw = true;

                    mCurrentObjectMouseOn = null;
                }
            }

            if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
            {
                if (mCanDraw && mCurrentOperateMode == OperateMode.Draw)
                {
                    GameObject groundPrefab = Resources.Load<GameObject>("Level/Ground");
                    GameObject groundObj = Instantiate(groundPrefab, transform);
                    groundObj.transform.position = mouseWorldPos;
                    groundObj.name = "Ground";
                }
                else if (mCurrentObjectMouseOn && mCurrentOperateMode == OperateMode.Erase)
                {
                    Destroy(mCurrentObjectMouseOn);
                }
            }
        }
    }
}
