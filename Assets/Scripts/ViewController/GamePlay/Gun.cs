using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Gun : MonoBehaviour, IController
    {
        private Bullet mBullet;
        // 获取枪械信息
        private GunInfo mGunInfo;
        private int mMaxBulletCount;

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }

        private void Awake()
        {
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();
            mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;
            mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunInfo.Name.Value));
        }

        public void Shoot()
        {
            // 如果枪械是一般状态
            if (mGunInfo.GunState.Value == GunState.Idle)
            {
                // 如果枪内有子弹
                if (mGunInfo.BulletCountInGun.Value > 0)
                {
                    var bullet = Instantiate(mBullet, mBullet.transform.position, mBullet.transform.rotation);
                    // 将全局的缩放值设置给子弹
                    bullet.transform.localScale = mBullet.transform.lossyScale;
                    bullet.gameObject.SetActive(true);

                    this.SendCommand(ShootCommand.Single);   // 每发送一次都会新建一个 Command，可以使用单例进行优化
                                                             // 如果对此命令系统进行了一些数据记录，则这样写会有一些问题
                }
                // 枪内没子弹
                else
                {
                    Reload();
                }
            }
        }

        public void Reload()
        {
            if (mGunInfo.GunState.Value == GunState.Idle &&
                mGunInfo.BulletCountInGun.Value != mMaxBulletCount &&
                mGunInfo.BulletCountOutGun.Value > 0)
            {
                this.SendCommand<ReloadCommand>();
            }
        }

        private void OnDestroy()
        {
            mGunInfo = null;
        }
    }
}
