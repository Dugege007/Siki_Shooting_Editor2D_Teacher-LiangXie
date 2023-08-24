using FrameworkDesign;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingEditor2D
{
    /// <summary>
    /// 枪械配置数据 接口
    /// </summary>
    public interface IGunConfigModel : IModel
    {
        GunConfigItem GetItemByName(string gunName);
    }

    /// <summary>
    /// 枪械配置表
    /// </summary>
    public class GunConfigItem
    {
        public GunConfigItem(string name, int bulletMaxCount, float attack, float frequency, float shootDistance, float recoil, bool needBullet, float reloadSeconds, string description)
        {
            Name = name;
            BulletMaxCount = bulletMaxCount;
            Attack = attack;
            Frequency = frequency;
            ShootDistance = shootDistance;
            Recoil = recoil;
            NeedBullet = needBullet;
            ReloadSeconds = reloadSeconds;
            Description = description;
        }

        public string Name { get; set; }
        public int BulletMaxCount { get; set; }
        public float Attack { get; set; }
        public float Frequency { get; set; }
        public float ShootDistance { get; set; }
        public float Recoil { get; set; }
        public bool NeedBullet { get; set; }
        public float ReloadSeconds { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 枪械配置数据 实现类
    /// </summary>
    public class GunConfigModel : AbstractModel, IGunConfigModel
    {
        protected override void OnInit()
        {

        }

        private Dictionary<string, GunConfigItem> mItems = new Dictionary<string, GunConfigItem>()
        {
            // 名称，弹夹子弹数，攻击力，攻击频率，射程，后坐力，是否需要子弹，换弹时间，描述
            { "手枪", new GunConfigItem("手枪", 7, 3, 3f, 0.5f, 0.1f, false, 1f, "威力偏小 射程中 射速中 后坐力小 无限弹药") },
            { "冲锋枪", new GunConfigItem("冲锋枪", 30, 2, 10f, 0.4f, 0.01f, true, 1f, "威力小 射程近 射速快 后坐力小") },
            { "霰弹枪", new GunConfigItem("霰弹枪", 2, 10, 1f, 0.333f, 0.5f, true, 2f, "威力大 射程近 射速慢 后坐力中 一次发射6-12发") },
            { "步枪", new GunConfigItem("步枪", 30, 5, 5f, 0.8f, 0.2f, true, 1.5f, "威力中 射程中 射速中 后坐力中") },
            { "狙击枪", new GunConfigItem("狙击枪", 10, 10, 0.5f, 1f, 1f, true, 1.5f, "威力大 射程远 射速慢 后坐力大 红外瞄准") },
            { "火箭筒", new GunConfigItem("火箭筒", 1, 15, 0.2f, 1f, 2f, true, 2f, "威力很大 射程远 射速很慢 后坐力很大 跟踪+爆炸") },
        };

        public GunConfigItem GetItemByName(string gunName)
        {
            return mItems[gunName];
        }
    }
}
