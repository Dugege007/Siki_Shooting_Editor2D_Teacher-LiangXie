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

            XmlNode levelNode = document.SelectSingleNode("Level");

            foreach (XmlElement levelItemNode in levelNode.ChildNodes)
            {

            }
        }
    }
}
