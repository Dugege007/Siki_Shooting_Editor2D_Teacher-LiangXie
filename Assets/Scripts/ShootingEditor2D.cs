using FrameworkDesign;

namespace ShootingEditor2D
{
    /// <summary>
    /// 2D 射击游戏架构类
    /// </summary>
    public class ShootingEditor2D : Architecture<ShootingEditor2D>
    {
        // 初始化架构
        protected override void Init()
        {
            // 将所需要的层注册到架构中

            // 系统层
            this.RegisterSystem<IStatSystem>(new StatSystem());
            this.RegisterSystem<IGunSystem>(new GunSystem());
            this.RegisterSystem<ITimeSystem>(new TimeSystem());

            // 数据层
            this.RegisterModel<IPlayerModel>(new PlayerModel());
        }
    }
}
