using FrameworkDesign;

namespace ShootingEditor2D
{
    public interface IStatSystem : ISystem
    {
        /// <summary>
        /// 击杀敌人数量
        /// </summary>
        BindableProperty<int> KillCount { get; }
    }

    // 实现类
    public class StatSystem : AbstractSystem, IStatSystem
    {
        public BindableProperty<int> KillCount { get; } = new BindableProperty<int>() { Value = 0 };

        protected override void OnInit()
        {
            
        }
    }
}
