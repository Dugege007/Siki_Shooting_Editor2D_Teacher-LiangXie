using FrameworkDesign;

namespace ShootingEditor2D
{
    public class ShootingEditor2D : Architecture<ShootingEditor2D>
    {
        protected override void Init()
        {
            // 注册到架构中
            // 系统层
            this.RegisterSystem<IStatSystem>(new StatSystem());

            // 数据层
            this.RegisterModel<IPlayerModel>(new PlayerModel());
        }
    }
}
