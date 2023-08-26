using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        private class LevelItemInfo
        {
            public string Name;
            public float X;
            public float Y;
        }

        public enum OperateMode
        {
            Draw,
            Erase
        }

        public enum BrushType
        {
            Ground,
            Player,
        }

        private OperateMode mCurrentOperateMode;
        private BrushType mCurrentBrushType = BrushType.Ground;

        public SpriteRenderer EmptyHighLight;
        private GameObject mCurrentObjectMouseOn;
        private bool mCanDraw;

        private Lazy<GUIStyle> mModelLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 50,
            alignment = TextAnchor.MiddleCenter,
        });

        private Lazy<GUIStyle> mLeftButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter,
        });

        private Lazy<GUIStyle> mRightButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 35,
            alignment = TextAnchor.MiddleCenter,
        });

        private void OnGUI()
        {
            Rect modeLabelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, 35, 600, 60);

            if (mCurrentOperateMode == OperateMode.Draw)
                GUI.Label(modeLabelRect, mCurrentOperateMode + ": " + mCurrentBrushType, mModelLabelStyle.Value);
            else
                GUI.Label(modeLabelRect, mCurrentOperateMode.ToString(), mModelLabelStyle.Value);

            Rect drawButtonRect = new Rect(20, 20, 200, 60);
            if (GUI.Button(drawButtonRect, "绘制", mLeftButtonStyle.Value))
                mCurrentOperateMode = OperateMode.Draw;

            Rect eraseButtonRect = new Rect(20, 100, 200, 60);
            if (GUI.Button(eraseButtonRect, "橡皮", mLeftButtonStyle.Value))
                mCurrentOperateMode = OperateMode.Erase;

            Rect saveButtonRect = new Rect(Screen.width - 220, 20, 200, 60);
            if (GUI.Button(saveButtonRect, "保存", mLeftButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Erase;
                Debug.Log("保存");

                List<LevelItemInfo> infos = new List<LevelItemInfo>(transform.childCount);
                foreach (Transform child in transform)
                {
                    infos.Add(new LevelItemInfo()
                    {
                        Name = child.name,
                        X = child.position.x,
                        Y = child.position.y,
                    });
                    Debug.Log($"Name: {child.name}\n" + $"X: {child.position.x} Y: {child.position.y}");
                }

                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");
                document.AppendChild(declaration);

                XmlElement level = document.CreateElement("level");
                document.AppendChild(level);

                foreach (var levelItemInfo in infos)
                {
                    XmlElement levelItem = document.CreateElement("levelItem");
                    levelItem.SetAttribute("name", levelItemInfo.Name);
                    levelItem.SetAttribute("x", levelItemInfo.X.ToString());
                    levelItem.SetAttribute("y", levelItemInfo.Y.ToString());
                    level.AppendChild(levelItem);
                }

                StringBuilder stringBuilder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(stringBuilder);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlTextWriter.Formatting = Formatting.Indented;
                document.WriteTo(xmlTextWriter);

                Debug.Log(stringBuilder.ToString());
            }

            if (mCurrentOperateMode == OperateMode.Draw)
            {
                Rect groundButtonRect = new Rect(Screen.width - 220, 100, 200, 60);
                if (GUI.Button(groundButtonRect, "地块", mRightButtonStyle.Value))
                    mCurrentBrushType = BrushType.Ground;

                Rect playerButtonRect = new Rect(Screen.width - 220, 180, 200, 60);
                if (GUI.Button(playerButtonRect, "玩家", mRightButtonStyle.Value))
                    mCurrentBrushType = BrushType.Player;
            }
        }

        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f); // 坐标取整（四舍五入）
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
                        EmptyHighLight.color = new Color(1, 0, 0, 0.5f);    // 红色
                    else if (mCurrentOperateMode == OperateMode.Erase)
                        EmptyHighLight.color = new Color(1, 0.5f, 0, 0.5f); // 橙色

                    mCanDraw = false;
                    mCurrentObjectMouseOn = hit.collider.gameObject;
                }
                else
                {
                    if (mCurrentOperateMode == OperateMode.Draw)
                        EmptyHighLight.color = new Color(1, 1, 1, 0.5f);    // 白色
                    else if (mCurrentOperateMode == OperateMode.Erase)
                        EmptyHighLight.color = new Color(0, 0, 1, 0.5f);    // 蓝色

                    mCanDraw = true;
                    mCurrentObjectMouseOn = null;
                }
            }

            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && GUIUtility.hotControl == 0)
            {
                if (mCanDraw && mCurrentOperateMode == OperateMode.Draw)
                {
                    if (mCurrentBrushType == BrushType.Ground)
                    {
                        GameObject groundPrefab = Resources.Load<GameObject>("Level/Ground");
                        GameObject groundObj = Instantiate(groundPrefab, transform);
                        groundObj.transform.position = mouseWorldPos;
                        groundObj.name = "Ground";
                    }
                    else if (mCurrentBrushType == BrushType.Player)
                    {
                        GameObject playerPrefab = Resources.Load<GameObject>("Level/Ground");
                        GameObject playerObj = Instantiate(playerPrefab, transform);
                        playerObj.transform.position = mouseWorldPos;
                        playerObj.name = "Player";

                        playerObj.GetComponent<SpriteRenderer>().color = Color.yellow;
                    }

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
