using FrameworkDesign;

namespace ShootingEditor2D
{
    /// <summary>
    /// 玩家数据 接口
    /// </summary>
    public interface IPlayerModel : IModel
    {
        /// <summary>
        /// 玩家生命值
        /// </summary>
        BindableProperty<int> HP { get; }
    }

    /// <summary>
    /// 玩家数据 实现类
    /// </summary>
    public class PlayerModel : AbstractModel, IPlayerModel
    {
        public BindableProperty<int> HP { get; } = new BindableProperty<int>() { Value = 3 };

        protected override void OnInit()
        {

        }
    }
}
