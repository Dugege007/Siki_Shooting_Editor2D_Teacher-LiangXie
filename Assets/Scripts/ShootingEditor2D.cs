using QFramework;

namespace ShootingEditor2D
{
    /// <summary>
    /// 2D 射击游戏架构类
    /// </summary>
    public class ShootingEditor2D : Architecture<ShootingEditor2D>
    {
        protected override void Init()
        {
            // 将所需要的层注册到架构中

            // 系统层
            // 统计系统
            this.RegisterSystem<IStatSystem>(new StatSystem());
            // 枪械系统
            this.RegisterSystem<IGunSystem>(new GunSystem());
            // 时间控制系统
            this.RegisterSystem<ITimeSystem>(new TimeSystem());

            // 数据层
            // 枪械配置表
            this.RegisterModel<IGunConfigModel>(new GunConfigModel());
            // 玩家数据
            this.RegisterModel<IPlayerModel>(new PlayerModel());
        }
    }
}
