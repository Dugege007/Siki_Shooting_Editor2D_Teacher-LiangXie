using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Gun : ShootingEditor2DController
    {
        private Bullet mBullet;
        // 获取枪械信息
        private GunInfo mGunInfo;
        private int mMaxBulletCount;

        private void Awake()
        {
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();
            mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;
            mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunInfo.Name.Value));
        }

        public void Shoot()
        {
            // 如果枪内有子弹 且 枪械是一般状态
            if (mGunInfo.BulletCountInGun.Value > 0 && 
                mGunInfo.GunState.Value == GunState.Idle)
            {
                var bullet = Instantiate(mBullet, mBullet.transform.position, mBullet.transform.rotation);
                // 将全局的缩放值设置给子弹
                bullet.transform.localScale = mBullet.transform.lossyScale;
                bullet.gameObject.SetActive(true);

                this.SendCommand(ShootCommand.Single);   // 每发送一次都会新建一个 Command，可以使用单例进行优化
                                                         // 如果对此命令系统进行了一些数据记录，则这样写会有一些问题
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

            // Debug.Log("装弹时切枪，再切回来时，会卡在 Reloading 状态");
            // 已修复
        }

        private void OnDestroy()
        {
            mGunInfo = null;
        }
    }
}
