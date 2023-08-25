using FrameworkDesign;

namespace ShootingEditor2D
{
    public class FullBulletCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();

            // ������ǰǹ���ӵ�
            gunSystem.CurrentGun.BulletCountInGun.Value = gunConfigModel.GetItemByName(gunSystem.CurrentGun.Name.Value).MaxBulletCount;

            // ��������ǹ���ӵ�
            foreach (var gunInfo in gunSystem.GunInfos)
            {
                gunInfo.BulletCountInGun.Value = gunConfigModel.GetItemByName(gunInfo.Name.Value).MaxBulletCount;
            }
        }
    }
}
