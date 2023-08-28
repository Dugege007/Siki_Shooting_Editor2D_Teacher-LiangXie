using QFramework;

namespace ShootingEditor2D
{
    /// <summary>
    /// 查询指定枪支的最大子弹数量
    /// </summary>
    public class MaxBulletCountQuery : AbstractQuery<int>
    {
        // 枪支名称
        private readonly string mGunName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gunName">要查询的枪支名称</param>
        public MaxBulletCountQuery(string gunName)
        {
            mGunName = gunName;
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <returns>返回指定枪支的最大子弹数量</returns>
        /// <remarks>
        /// 通过枪支名称从 IGunConfigModel 中获取枪支配置项。
        /// </remarks>
        protected override int OnDo()   // 查询操作通常命名为 Do 做查询
        {
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();
            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.MaxBulletCount;
        }
    }
}
