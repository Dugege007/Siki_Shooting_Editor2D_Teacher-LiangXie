using FrameworkDesign;
using System;

namespace ShootingEditor2D
{
    public enum GunState
    {
        Idle,
        Shooting,
        Reloading,
        EmptyBullet,
        CoolDown,
    }

    public class GunInfo
    {
        /// <summary>
        /// 枪械名称
        /// </summary>
        public BindableProperty<string> Name;

        /// <summary>
        /// 枪械状态
        /// </summary>
        public BindableProperty<GunState> GunState; // 之前框架中的 BindableProperty<T> T类型限定为可比较类型，然而没必要

        /// <summary>
        /// 枪内子弹数量
        /// </summary>
        public BindableProperty<int> BulletCountInGun;

        /// <summary>
        /// 枪外子弹数量
        /// </summary>
        public BindableProperty<int> BulletCountOutGun;
    }
}
