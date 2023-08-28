using QFramework;

namespace ShootingEditor2D
{
    public class ShootCommand : AbstractCommand
    {
        // �Դ����������Ż�
        // ���������� new �Ĵ���
        public static readonly ShootCommand Single = new ShootCommand();

        protected override void OnExecute()
        {
            // ��ȡϵͳ
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            ITimeSystem timeSystem = this.GetSystem<ITimeSystem>();

            gunSystem.CurrentGun.BulletCountInGun.Value--;
            gunSystem.CurrentGun.GunState.Value = GunState.Shooting;

            // ��ȡģ��
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
