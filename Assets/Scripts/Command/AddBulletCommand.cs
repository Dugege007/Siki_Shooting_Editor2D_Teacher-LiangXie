using QFramework;

namespace ShootingEditor2D
{
    public class AddBulletCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();

            AddBullet(gunSystem.CurrentGun, gunConfigModel);

            foreach (GunInfo gunInfo in gunSystem.GunInfos)
            {
                AddBullet(gunInfo, gunConfigModel);
            }
        }

        /// <summary>
        /// 增加子弹
        /// </summary>
        /// <param name="gunInfo">枪械信息</param>
        /// <param name="gunConfigModel">枪械配置数据</param>
        private void AddBullet(GunInfo gunInfo, IGunConfigModel gunConfigModel)
        {
            // 根据枪械信息中的枪械名称，获取枪械配置数据中的该枪配置
            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(gunInfo.Name.Value);

            if (gunConfigItem.NeedBullet == false) return;

            // 根据枪械配置中的弹夹子弹数量，给该枪外子弹增加数量
            gunInfo.BulletCountOutGun.Value += gunConfigItem.MaxBulletCount;
        }
    }
}
