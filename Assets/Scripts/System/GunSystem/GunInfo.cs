using FrameworkDesign;
using System;

namespace ShootingEditor2D
{
    public enum GunState
    {
        Idle,
        Shooting,
        Reload,
        EmptyBullet,
        CoolDown,
    }

    public class GunInfo
    {
        [Obsolete("请使用 BulletCountInGun", false)]   // 提示该字段已弃用，显示警告或报错；团队协作中可以平滑升级过程，避免冲突
        /// <summary>
        /// 子弹数量（已代理枪内子弹数量）
        /// </summary>
        public BindableProperty<int> BulletCount
        {
            get { return BulletCountInGun; }
            set { BulletCountInGun = value; }
        }

        /// <summary>
        /// 枪内子弹数量
        /// </summary>
        public BindableProperty<int> BulletCountInGun;

        /// <summary>
        /// 枪械状态
        /// </summary>
        public BindableProperty<GunState> GunState; // 之前框架中的 BindableProperty<T> T类型限定为可比较类型，然而没必要

        /// <summary>
        /// 枪外子弹数量
        /// </summary>
        public BindableProperty<int> BulletCountOutGun;
    }
}
