using FrameworkDesign;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public class UIController : MonoBehaviour, IController
    {
        private IPlayerModel mPlayerModel;
        private IStatSystem mIStatSystem;

        // 懒加载，当第一次调用 mLabelStyle 时，会执行 = 右边的初始化
        private readonly Lazy<GUIStyle> mLabelStyle = new(() =>
        {
            // 默认 Style
            return new GUIStyle(GUI.skin.label)
            {
                // 修改字体大小
                fontSize = 40
            };
        });

        public IArchitecture GetArchiteccture()
        {
            return ShootingEditor2D.Interface;
        }

        private void Awake()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
            mIStatSystem = this.GetSystem<IStatSystem>();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 100), $"生命值：{mPlayerModel.HP.Value}/3", mLabelStyle.Value);
            GUI.Label(new Rect(Screen.width - 10 - 300, 10, 300, 100), $"击杀次数：{mIStatSystem.KillCount.Value}", mLabelStyle.Value);
        }
    }
}
