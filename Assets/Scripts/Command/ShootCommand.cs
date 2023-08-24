using FrameworkDesign;

namespace ShootingEditor2D
{
    public class ShootCommand : AbstractCommand
    {
        // 对此命名进行优化
        // 单例，减少 new 的次数
        public static readonly ShootCommand Single = new ShootCommand();

        protected override void OnExecute()
        {
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();

            gunSystem.CurrentGun.BulletCountInGun.Value--;
            gunSystem.CurrentGun.GunState.Value = GunState.Shooting;
        }
    }
}
