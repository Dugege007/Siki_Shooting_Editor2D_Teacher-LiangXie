
using QFramework;

namespace ShootingEditor2D
{
    public class PickGunCommand : AbstractCommand
    {
        private readonly string mName;
        private readonly int mBulletInGun;
        private readonly int mBulletOutGun;

        /// <summary>
        /// 捡枪命令
        /// </summary>
        /// <param name="name">枪械名称</param>
        /// <param name="bulletInGun">枪内子弹</param>
        /// <param name="bulletOutGun">枪外子弹</param>
        public PickGunCommand(string name, int bulletInGun, int bulletOutGun)
        {
            mName = name;
            mBulletInGun = bulletInGun;
            mBulletOutGun = bulletOutGun;
        }

        protected override void OnExecute()
        {
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            gunSystem.PickGun(mName, mBulletInGun, mBulletOutGun);
        }
    }
}
