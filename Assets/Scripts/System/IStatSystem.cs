using FrameworkDesign;

namespace ShootingEditor2D
{
    /// <summary>
    /// 统计系统 接口
    /// </summary>
    public interface IStatSystem : ISystem
    {
        /// <summary>
        /// 击杀敌人数量
        /// </summary>
        BindableProperty<int> KillCount { get; }
    }

    /// <summary>
    /// 统计系统 实现类
    /// </summary>
    public class StatSystem : AbstractSystem, IStatSystem
    {
        public BindableProperty<int> KillCount { get; } = new BindableProperty<int>() { Value = 0 };

        protected override void OnInit()
        {
            
        }
    }
}
