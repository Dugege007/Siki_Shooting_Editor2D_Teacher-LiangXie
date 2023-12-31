using QFramework;
using System;
using UnityEngine;

namespace ShootingEditor2D
{
    public class UIController : ShootingEditor2DController
    {
        private IStatSystem mIStatSystem;
        private IGunSystem mGunSystem;

        private IPlayerModel mPlayerModel;
        private IGunConfigModel mGunConfigModel;

        private int mMaxBulletCount;

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

        private void Awake()
        {
            // 获取系统
            mIStatSystem = this.GetSystem<IStatSystem>();
            mGunSystem = this.GetSystem<IGunSystem>();

            // 获取数据
            mPlayerModel = this.GetModel<IPlayerModel>();

            // 查询枪械信息
            mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunSystem.CurrentGun.Name.Value));

            // 监听事件
            // 监听当前枪械变更
            this.RegisterEvent<OnCurrentGunChanged>(e =>
            {
                mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunSystem.CurrentGun.Name.Value));
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 500, 100), $"生命：{mPlayerModel.HP.Value}/3", mLabelStyle.Value);
            GUI.Label(new Rect(10, 60, 500, 100), $"枪械名称：{mGunSystem.CurrentGun.Name.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 110, 500, 100), $"枪械状态：{mGunSystem.CurrentGun.GunState.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 160, 500, 100), $"枪内子弹：{mGunSystem.CurrentGun.BulletCountInGun.Value}/{mMaxBulletCount}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 210, 300, 100), $"枪外子弹：{mGunSystem.CurrentGun.BulletCountOutGun.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(Screen.width - 10 - 300, 10, 300, 100), $"击杀：{mIStatSystem.KillCount.Value}", mLabelStyle.Value);
        }

        private void OnDestroy()
        {
            mPlayerModel = null;
            mIStatSystem = null;
            mGunSystem = null;
        }
    }
}
