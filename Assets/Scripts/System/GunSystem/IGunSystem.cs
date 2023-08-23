using FrameworkDesign;

namespace ShootingEditor2D
{
    /// <summary>
    /// 枪械系统 接口
    /// </summary>
    public interface IGunSystem : ISystem
    {
        /// <summary>
        /// 当前枪械信息
        /// </summary>
        GunInfo CurrentGun { get; }
    }

    /// <summary>
    /// 枪械系统 实现类
    /// </summary>
    public class GunSystem : AbstractSystem, IGunSystem
    {
        protected override void OnInit()
        {

        }

        public GunInfo CurrentGun { get; } = new GunInfo()
        {
            BulletCount = new BindableProperty<int>() { Value = 3 }
        };
    }
}
