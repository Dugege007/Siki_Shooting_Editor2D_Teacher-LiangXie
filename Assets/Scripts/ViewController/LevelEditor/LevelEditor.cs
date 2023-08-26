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
            public string Path;
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

        private Transform levelTrans;
        private Transform charactersTrans;

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

        private void Awake()
        {
            levelTrans = transform.Find("Level");
            charactersTrans = transform.Find("Characters");
        }

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

                SaveXML();
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
                    string resourcePath = "Level/Ground";
                    if (mCurrentBrushType == BrushType.Ground)
                    {
                        GameObject groundPrefab = Resources.Load<GameObject>(resourcePath);
                        GameObject groundObj = Instantiate(groundPrefab, transform);
                        groundObj.transform.position = mouseWorldPos;
                        groundObj.name = "Ground";
                        groundObj.transform.SetParent(levelTrans);
                    }
                    else if (mCurrentBrushType == BrushType.Player)
                    {
                        GameObject playerPrefab = Resources.Load<GameObject>(resourcePath);
                        GameObject playerObj = Instantiate(playerPrefab, transform);
                        playerObj.transform.position = mouseWorldPos;
                        playerObj.name = "Player";
                        playerObj.transform.SetParent(charactersTrans);

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

        private void SaveXML()
        {
            List<LevelItemInfo> characterInfos = GetLevelItemInfos(charactersTrans);
            List<LevelItemInfo> levelInfos = GetLevelItemInfos(levelTrans);

            XmlDocument document = new XmlDocument();
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");
            document.AppendChild(declaration);

            XmlElement level = document.CreateElement("level");
            document.AppendChild(level);

            SetItemInfoAttributes(characterInfos, document, level);
            SetItemInfoAttributes(levelInfos, document, level);

            string levelFilesFolder = Application.persistentDataPath + "/LevelFiles";
            Debug.Log(levelFilesFolder);

            if (!Directory.Exists(levelFilesFolder))
                Directory.CreateDirectory(levelFilesFolder);

            string levelFilePath = levelFilesFolder + "/" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_level_data.xml";

            document.Save(levelFilePath);
        }

        private List<LevelItemInfo> GetLevelItemInfos(Transform itemTypeTrans)
        {
            List<LevelItemInfo> itemInfos = new List<LevelItemInfo>(charactersTrans.childCount);

            foreach (Transform child in itemTypeTrans)
            {
                itemInfos.Add(new LevelItemInfo()
                {
                    Path = itemTypeTrans.name,
                    Name = child.name,
                    X = child.position.x,
                    Y = child.position.y,
                });

                Debug.Log($"Name: {child.name}\n" + $"X: {child.position.x} Y: {child.position.y}");
            }

            return itemInfos;
        }

        private void SetItemInfoAttributes(List<LevelItemInfo> itemInfos, XmlDocument document, XmlElement level)
        {

            foreach (var levelItemInfo in itemInfos)
            {
                XmlElement levelItem = document.CreateElement("levelItem");
                levelItem.SetAttribute("name", levelItemInfo.Path + "/" + levelItemInfo.Name);
                levelItem.SetAttribute("x", levelItemInfo.X.ToString());
                levelItem.SetAttribute("y", levelItemInfo.Y.ToString());
                level.AppendChild(levelItem);
            }
        }
    }
}
