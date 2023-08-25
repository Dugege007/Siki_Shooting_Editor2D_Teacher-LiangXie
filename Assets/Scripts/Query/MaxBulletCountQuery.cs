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

        public int Do() // ��ѯ����ͨ������Ϊ Do ����ѯ
        {
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();
            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.MaxBulletCount;
        }
    }
}
