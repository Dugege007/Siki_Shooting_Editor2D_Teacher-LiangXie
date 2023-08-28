using QFramework;

namespace ShootingEditor2D
{

    public class ReloadCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            GunInfo currentGun = this.GetSystem<IGunSystem>().CurrentGun;
            GunConfigItem gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(currentGun.Name.Value);

            // 需要装弹数
            int needBulletCount = gunConfigItem.MaxBulletCount - currentGun.BulletCountInGun.Value;

            // 如果需要装弹
            if (needBulletCount > 0)
            {
                // 切换到换弹状态
                currentGun.GunState.Value = GunState.Reloading;

                // 经过装弹时间后执行回调
                this.GetSystem<ITimeSystem>().AddDelayTask(gunConfigItem.ReloadSeconds, () =>
                {
                    // 如果枪外子弹量充足
                    if (currentGun.BulletCountOutGun.Value >= needBulletCount)
                    {
                        currentGun.BulletCountInGun.Value += needBulletCount;
                        currentGun.BulletCountOutGun.Value -= needBulletCount;
                    }
                    // 不充足
                    else
                    {
                        currentGun.BulletCountInGun.Value += currentGun.BulletCountOutGun.Value;
                        currentGun.BulletCountOutGun.Value = 0;
                    }

                    // 切换到正常状态
                    currentGun.GunState.Value = GunState.Idle;
                });
            }
        }
    }
}
