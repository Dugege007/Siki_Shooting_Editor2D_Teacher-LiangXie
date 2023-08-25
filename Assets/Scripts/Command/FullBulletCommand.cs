using FrameworkDesign;

namespace ShootingEditor2D
{
    public class FullBulletCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();

            // 填满当前枪内子弹
            gunSystem.CurrentGun.BulletCountInGun.Value = gunConfigModel.GetItemByName(gunSystem.CurrentGun.Name.Value).MaxBulletCount;

            // 填满其他枪内子弹
            foreach (var gunInfo in gunSystem.GunInfos)
            {
                gunInfo.BulletCountInGun.Value = gunConfigModel.GetItemByName(gunInfo.Name.Value).MaxBulletCount;
            }
        }
    }
}
