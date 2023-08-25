using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkDesign;

namespace ShootingEditor2D
{
    public class MaxBulletCountQuery : IBelongToArchitecture, ICanGetModel
    {
        private readonly string mGunName;

        public MaxBulletCountQuery(string gunName)
        {
            mGunName = gunName;
        }

        public IArchitecture GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }

        public int Do() // 查询操作通常命名为 Do 做查询
        {
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();
            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.MaxBulletCount;
        }
    }
}
