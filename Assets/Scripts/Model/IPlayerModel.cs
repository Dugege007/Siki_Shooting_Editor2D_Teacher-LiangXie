using FrameworkDesign;

namespace ShootingEditor2D
{
    public interface IPlayerModel : IModel
    {
        // 玩家生命值
        BindableProperty<int> HP { get; }
    }

    public class PlayerModel : AbstractModel, IPlayerModel
    {
        public BindableProperty<int> HP { get; } = new BindableProperty<int>() { Value = 3 };

        protected override void OnInit()
        {

        }
    }
}
