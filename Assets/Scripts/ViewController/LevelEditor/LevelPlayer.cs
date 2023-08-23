using System.Xml;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelPlayer : MonoBehaviour
    {
        public TextAsset LevelFile;

        private void Start()
        {
            string xml = LevelFile.text;
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            XmlNode levelNode = document.SelectSingleNode("level");

            foreach (XmlElement levelItemNode in levelNode.ChildNodes)
            {
                // ���� XML �ļ�
                string levelItemName = levelItemNode.Attributes["name"].Value;
                float levelItemX = float.Parse(levelItemNode.Attributes["x"].Value);
                float levelItemY = float.Parse(levelItemNode.Attributes["y"].Value);
                Debug.Log(levelItemName + ": (" + levelItemY + ", " + levelItemX + ")");

                // ������Ӧ��Ԥ����
                GameObject levelItemPrefab = Resources.Load<GameObject>(levelItemName);
                GameObject levelItemGameObj = Instantiate(levelItemPrefab, transform);
                levelItemGameObj.transform.position = new Vector3(levelItemX, levelItemY, 0);
            }
        }
    }
}
