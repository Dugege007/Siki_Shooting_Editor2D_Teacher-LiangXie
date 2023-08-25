
using FrameworkDesign;

namespace ShootingEditor2D
{
    public class PickGunCommand : AbstractCommand
    {
        private readonly string mName;
        private readonly int mBulletInGun;
        private readonly int mBulletOutGun;

        public PickGunCommand(string name, int bulletInGun, int bulletOutGun)
        {
            mName = name;
            mBulletInGun = bulletInGun;
            mBulletOutGun = bulletOutGun;
        }

        protected override void OnExecute()
        {
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            gunSystem.PickGun(mName, mBulletOutGun, mBulletOutGun);
        }
    }
}
