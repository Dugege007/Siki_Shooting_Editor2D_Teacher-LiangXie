using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelPlayer : MonoBehaviour
    {
        private enum State
        {
            Selection,
            Playing,
        }

        private State mCurrentState = State.Selection;

        private static string mLevelFilesFolder;

        private void Awake()
        {
            mLevelFilesFolder = Application.persistentDataPath + "/LevelFiles";
        }

        private void ParseAndRun(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            XmlNode levelNode = document.SelectSingleNode("level");

            foreach (XmlElement levelItemNode in levelNode.ChildNodes)
            {
                // 解析 XML 文件
                string levelItemName = levelItemNode.Attributes["name"].Value;
                float levelItemX = float.Parse(levelItemNode.Attributes["x"].Value);
                float levelItemY = float.Parse(levelItemNode.Attributes["y"].Value);
                Debug.Log(levelItemName + ": (" + levelItemY + ", " + levelItemX + ")");

                // 加载相应的预制体
                GameObject levelItemPrefab = Resources.Load<GameObject>(levelItemName);
                if (levelItemPrefab != null)
                {
                    GameObject levelItemGameObj = Instantiate(levelItemPrefab, transform);
                    levelItemGameObj.transform.position = new Vector3(levelItemX, levelItemY, 0);
                }
            }
        }

        private void OnGUI()
        {
            if (mCurrentState == State.Selection)
            {
                string[] filePaths = Directory.GetFiles(mLevelFilesFolder);

                int y = 10;

                foreach (string filePath in filePaths.Where(f => f.EndsWith("xml")))
                {
                    string fileName = Path.GetFileName(filePath);

                    if (GUI.Button(new Rect(10, y, 100, 40), fileName))
                    {
                        string xml = File.ReadAllText(filePath);
                        ParseAndRun(xml);
                        mCurrentState = State.Playing;
                    }

                    y += 50;
                }
            }
        }
    }
}
