using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class UIGameStart : MonoBehaviour
    {
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 80,
            alignment = TextAnchor.MiddleCenter,
        });

        public readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter,
        });

        private void OnGUI()
        {
            float labelWidth = 800;
            float labelHeight = 120;
            Vector2 labelSize = new Vector2(labelWidth, labelHeight);
            Vector2 labelPosition = new Vector2(Screen.width, Screen.height) * 0.5f - labelSize * 0.5f;
            Rect labelRect = new Rect(labelPosition, labelSize);
            GUI.Label(labelRect, "ShootingEditor2D", mLabelStyle.Value);

            float buttonWidth = 300;
            float buttonHeight = 100;
            Vector2 buttonSize = new Vector2(buttonWidth, buttonHeight);
            Vector2 buttonPosition = new Vector2(Screen.width, Screen.height) * 0.5f - buttonSize * 0.5f + Vector2.up * 150f;
            Rect buttonRect = new Rect(buttonPosition, buttonSize);

            if (GUI.Button(buttonRect,"¿ªÊ¼ÓÎÏ·",mButtonStyle.Value))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
