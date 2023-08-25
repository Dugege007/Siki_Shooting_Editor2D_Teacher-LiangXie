using FrameworkDesign;
using System.Collections.Generic;
using System.Linq;

namespace ShootingEditor2D
{
    /// <summary>
    /// 枪械系统 接口
    /// </summary>
    public interface IGunSystem : ISystem
    {
        GunInfo CurrentGun { get; }

        void PickGun(string name, int bulletCountInGun, int bulletCountOutGun);
    }

    public struct OnCurrentGunChanged
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// 枪械系统 实现类
    /// </summary>
    public class GunSystem : AbstractSystem, IGunSystem
    {
        protected override void OnInit()
        {

        }

        /// <summary>
        /// 当前枪械信息
        /// </summary>
        public GunInfo CurrentGun { get; } = new GunInfo()
        {
            Name = new BindableProperty<string>() { Value = "手枪" },
            GunState = new BindableProperty<GunState>() { Value = GunState.Idle },
            BulletCountInGun = new BindableProperty<int>() { Value = 3 },
            BulletCountOutGun = new BindableProperty<int>() { Value = 15 },
        };

        private Queue<GunInfo> mGunInfos = new Queue<GunInfo>();

        public void PickGun(string name, int bulletCountInGun, int bulletCountOutGun)
        {
            // 如果与当前枪械相同
            if (CurrentGun.Name.Value == name)
            {
                CurrentGun.BulletCountOutGun.Value += bulletCountInGun;
                CurrentGun.BulletCountOutGun.Value += bulletCountOutGun;
            }
            // 如果与缓存的枪械相同
            else if(mGunInfos.Any(gunInfo => gunInfo.Name.Value == name))
            {
                // 从缓存中取出枪械信息
                // .First() 方法返回序列中满足条件的第一个元素
                GunInfo gunInfo = mGunInfos.First(gunInfo => gunInfo.Name.Value == name);
                gunInfo.BulletCountOutGun.Value += bulletCountInGun;
                gunInfo.BulletCountOutGun.Value += bulletCountOutGun;
            }
            // 如果是一把新枪
            else
            {
                // 缓存当前枪械信息
                GunInfo currentGunInfo = new GunInfo()
                {
                    Name = new BindableProperty<string> { Value = CurrentGun.Name.Value },
                    GunState = new BindableProperty<GunState> { Value = CurrentGun.GunState.Value },
                    BulletCountInGun = new BindableProperty<int> { Value = CurrentGun.BulletCountInGun.Value },
                    BulletCountOutGun = new BindableProperty<int> { Value = CurrentGun.BulletCountOutGun.Value },
                };
                // 缓存到队列
                mGunInfos.Enqueue(currentGunInfo);

                // 修改当前枪械信息为刚捡到的枪的信息
                CurrentGun.Name.Value = name;
                CurrentGun.GunState.Value = GunState.Idle;
                CurrentGun.BulletCountInGun.Value = bulletCountInGun;
                CurrentGun.BulletCountOutGun.Value = bulletCountOutGun;

                // 发送事件给表现层，以更新枪械信息 UI
                this.SendEvent(new OnCurrentGunChanged()
                {
                    Name = name,
                });
            }
        }
    }
}
