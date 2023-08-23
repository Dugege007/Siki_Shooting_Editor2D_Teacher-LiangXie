using FrameworkDesign;

namespace ShootingEditor2D
{
    public class ShootCommand : AbstractCommand
    {
        // �Դ����������Ż�
        // ���������� new �Ĵ���
        public static readonly ShootCommand Single = new ShootCommand();

        protected override void OnExecute()
        {
            this.GetSystem<IGunSystem>().CurrentGun.BulletCount.Value--;
        }
    }
}
