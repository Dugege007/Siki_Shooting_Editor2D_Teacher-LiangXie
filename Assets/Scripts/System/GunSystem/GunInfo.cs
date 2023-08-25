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
        /// ǹе����
        /// </summary>
        public BindableProperty<string> Name;

        /// <summary>
        /// ǹе״̬
        /// </summary>
        public BindableProperty<GunState> GunState; // ֮ǰ����е� BindableProperty<T> T�����޶�Ϊ�ɱȽ����ͣ�Ȼ��û��Ҫ

        /// <summary>
        /// ǹ���ӵ�����
        /// </summary>
        public BindableProperty<int> BulletCountInGun;

        /// <summary>
        /// ǹ���ӵ�����
        /// </summary>
        public BindableProperty<int> BulletCountOutGun;
    }
}
