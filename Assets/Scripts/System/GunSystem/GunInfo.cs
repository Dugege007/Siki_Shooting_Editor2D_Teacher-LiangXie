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
        [Obsolete("��ʹ�� BulletCountInGun", false)]   // ��ʾ���ֶ������ã���ʾ����򱨴��Ŷ�Э���п���ƽ���������̣������ͻ
        /// <summary>
        /// �ӵ��������Ѵ���ǹ���ӵ�������
        /// </summary>
        public BindableProperty<int> BulletCount
        {
            get { return BulletCountInGun; }
            set { BulletCountInGun = value; }
        }

        /// <summary>
        /// ǹ���ӵ�����
        /// </summary>
        public BindableProperty<int> BulletCountInGun;

        /// <summary>
        /// ǹе״̬
        /// </summary>
        public BindableProperty<GunState> GunState; // ֮ǰ����е� BindableProperty<T> T�����޶�Ϊ�ɱȽ����ͣ�Ȼ��û��Ҫ

        /// <summary>
        /// ǹ���ӵ�����
        /// </summary>
        public BindableProperty<int> BulletCountOutGun;
    }
}
