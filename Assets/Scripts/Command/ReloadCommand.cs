using QFramework;

namespace ShootingEditor2D
{

    public class ReloadCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            GunInfo currentGun = this.GetSystem<IGunSystem>().CurrentGun;
            GunConfigItem gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(currentGun.Name.Value);

            // ��Ҫװ����
            int needBulletCount = gunConfigItem.MaxBulletCount - currentGun.BulletCountInGun.Value;

            // �����Ҫװ��
            if (needBulletCount > 0)
            {
                // �л�������״̬
                currentGun.GunState.Value = GunState.Reloading;

                // ����װ��ʱ���ִ�лص�
                this.GetSystem<ITimeSystem>().AddDelayTask(gunConfigItem.ReloadSeconds, () =>
                {
                    // ���ǹ���ӵ�������
                    if (currentGun.BulletCountOutGun.Value >= needBulletCount)
                    {
                        currentGun.BulletCountInGun.Value += needBulletCount;
                        currentGun.BulletCountOutGun.Value -= needBulletCount;
                    }
                    // ������
                    else
                    {
                        currentGun.BulletCountInGun.Value += currentGun.BulletCountOutGun.Value;
                        currentGun.BulletCountOutGun.Value = 0;
                    }

                    // �л�������״̬
                    currentGun.GunState.Value = GunState.Idle;
                });
            }
        }
    }
}
