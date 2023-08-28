using QFramework;

namespace ShootingEditor2D
{
    public class ShootCommand : AbstractCommand
    {
        // 对此命名进行优化
        // 单例，减少 new 的次数
        public static readonly ShootCommand Single = new ShootCommand();

        protected override void OnExecute()
        {
            // 获取系统
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            ITimeSystem timeSystem = this.GetSystem<ITimeSystem>();

            gunSystem.CurrentGun.BulletCountInGun.Value--;
            gunSystem.CurrentGun.GunState.Value = GunState.Shooting;

            // 获取模型
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();
            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(gunSystem.CurrentGun.Name.Value);

            timeSystem.AddDelayTask(1 / gunConfigItem.Frequency, () =>
            {
                gunSystem.CurrentGun.GunState.Value = GunState.Idle;

                if (gunSystem.CurrentGun.BulletCountInGun.Value == 0 && gunSystem.CurrentGun.BulletCountOutGun.Value > 0)
                {
                    this.SendCommand<ReloadCommand>();
                }
            });
        }
    }
}
