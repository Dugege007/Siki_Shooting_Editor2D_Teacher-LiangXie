using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class UIGamePass : MonoBehaviour
    {
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle()
        {
            fontSize = 80,
            alignment = TextAnchor.MiddleCenter
        });

        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle()
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        });

        private void OnGUI()
        {
            // 绘制 游戏通关 标题
            var labelWidth = 600;
            var labelHeight = 120;
            var labelPosition = new Vector2(Screen.width - labelWidth, Screen.height - labelHeight) * 0.5f;
            var labelSize = new Vector2(labelWidth, labelHeight);
            var labelRect = new Rect(labelPosition, labelSize);

            GUI.Label(labelRect, "游戏通关", mLabelStyle.Value);

            // 绘制 返回首页 按钮
            var buttonWidth = 200;
            var buttonHeight = 100;
            var buttonPostion = new Vector2(Screen.width, Screen.height) * 0.5f - new Vector2(buttonWidth, buttonHeight) * 0.5f + new Vector2(0, 150);
            var buttonSize=new Vector2(buttonWidth, buttonHeight);
            var buttonRext = new Rect(buttonPostion, buttonSize);

            if(GUI.Button(buttonRext, "返回首页", mButtonStyle.Value))
            {
                SceneManager.LoadScene("GameStart");
            }
        }
    }
}
