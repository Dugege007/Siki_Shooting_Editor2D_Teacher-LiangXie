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

        private OperateMode mCurrentOperateMode;

        public SpriteRenderer EmptyHighLight;
        private GameObject mCurrentObjectMouseOn;
        private bool mCanDraw;

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

            Rect drawButtonRect = new Rect(20, 20, 200, 60);
            if (GUI.Button(drawButtonRect, "����", mButtonStyle.Value))
                mCurrentOperateMode = OperateMode.Draw;

            Rect eraseButtonRect = new Rect(20, 100, 200, 60);
            if (GUI.Button(eraseButtonRect, "��Ƥ", mButtonStyle.Value))
                mCurrentOperateMode = OperateMode.Erase;
        }

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f); // ����ȡ�����������룩
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            mouseWorldPos.z = 0;

            if (GUIUtility.hotControl == 0)
                EmptyHighLight.gameObject.SetActive(true);
            else
                EmptyHighLight.gameObject.SetActive(false);

            if (Mathf.Abs(EmptyHighLight.transform.position.x - mouseWorldPos.x) < 0.1f &&
                Mathf.Abs(EmptyHighLight.transform.position.y - mouseWorldPos.y) < 0.1f)
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
                        EmptyHighLight.color = new Color(1, 0, 0, 0.5f);    // ��ɫ
                    else if (mCurrentOperateMode == OperateMode.Erase)
                        EmptyHighLight.color = new Color(1, 0.5f, 0, 0.5f); // ��ɫ

                    mCanDraw = false;
                    mCurrentObjectMouseOn = hit.collider.gameObject;
                }
                else
                {
                    if (mCurrentOperateMode == OperateMode.Draw)
                        EmptyHighLight.color = new Color(1, 1, 1, 0.5f);    // ��ɫ
                    else if (mCurrentOperateMode == OperateMode.Erase)
                        EmptyHighLight.color = new Color(0, 0, 1, 0.5f);    // ��ɫ

                    mCanDraw = true;
                    mCurrentObjectMouseOn = null;
                }
            }

            if (Input.GetMouseButtonDown(0) ||
                Input.GetMouseButton(0) &&
                GUIUtility.hotControl == 0)
            {
                if (mCanDraw && mCurrentOperateMode == OperateMode.Draw)
                {
                    GameObject groundPrefab = Resources.Load<GameObject>("Level/Ground");
                    GameObject groundObj = Instantiate(groundPrefab, transform);
                    groundObj.transform.position = mouseWorldPos;
                    groundObj.name = "Ground";

                    mCanDraw = false;
                }
                else if (mCurrentObjectMouseOn && mCurrentOperateMode == OperateMode.Erase)
                {
                    Destroy(mCurrentObjectMouseOn);

                    mCurrentObjectMouseOn = null;
                }
            }
        }
    }
}
