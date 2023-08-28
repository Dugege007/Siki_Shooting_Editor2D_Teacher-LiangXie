using QFramework;
using Mono.Cecil;
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

        Queue<GunInfo> GunInfos { get; }

        void PickGun(string name, int bulletCountInGun, int bulletCountOutGun);

        void ShiftGun();
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
        public Queue<GunInfo> GunInfos { get { return mGunInfos; } }

        public void PickGun(string name, int bulletCountInGun, int bulletCountOutGun)
        {
            // 如果与当前枪械相同
            if (CurrentGun.Name.Value == name)
            {
                CurrentGun.BulletCountOutGun.Value += bulletCountInGun;
                CurrentGun.BulletCountOutGun.Value += bulletCountOutGun;
            }
            // 如果与缓存的枪械相同
            else if (mGunInfos.Any(gunInfo => gunInfo.Name.Value == name))
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
                // 当前枪信息入队，并将下把枪的信息赋给当前枪
                EnqueueCurrentGun(name, GunState.Idle, bulletCountInGun, bulletCountOutGun);
            }
        }

        public void ShiftGun()
        {
            if (mGunInfos.Count == 0) return;

            // 下把枪出队
            GunInfo preGunInfo = mGunInfos.Dequeue();
            // 当前枪信息入队，并将下把枪的信息赋给当前枪
            EnqueueCurrentGun(preGunInfo.Name.Value, preGunInfo.GunState.Value, preGunInfo.BulletCountInGun.Value, preGunInfo.BulletCountOutGun.Value);
        }

        /// <summary>
        /// 缓存当前枪械信息，并将下把枪信息赋给当前枪
        /// </summary>
        /// <param name="nextGunName">枪械名称</param>
        /// <param name="nextGunState">枪械状态</param>
        /// <param name="nextBulletInGun">枪内子弹数</param>
        /// <param name="nextBulletOutGun">枪外子弹数</param>
        private void EnqueueCurrentGun(string nextGunName, GunState nextGunState, int nextBulletInGun, int nextBulletOutGun)
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
            CurrentGun.Name.Value = nextGunName;
            CurrentGun.GunState.Value = nextGunState;
            CurrentGun.BulletCountInGun.Value = nextBulletInGun;
            CurrentGun.BulletCountOutGun.Value = nextBulletOutGun;

            // 如果切回时是换弹状态
            if (CurrentGun.GunState.Value == GunState.Reloading)
                ReloadBullet();
            // 如果切回时是射击状态
            else if (CurrentGun.GunState.Value == GunState.Shooting)
                ContinueShooting();

            // 发送事件给表现层，以更新枪械信息 UI
            this.SendEvent(new OnCurrentGunChanged()
            {
                Name = nextGunName,
            });
        }

        private void ReloadBullet()
        {
            ITimeSystem timeSystem = this.GetSystem<ITimeSystem>();
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();

            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(CurrentGun.Name.Value);
            float reloadSeconds = gunConfigItem.ReloadSeconds;
            int needBulletCount = gunConfigItem.MaxBulletCount - CurrentGun.BulletCountInGun.Value;

            // 重新换弹
            timeSystem.AddDelayTask(reloadSeconds, () =>
            {
                // 如果枪外子弹量充足
                if (CurrentGun.BulletCountOutGun.Value >= needBulletCount)
                {
                    CurrentGun.BulletCountInGun.Value += needBulletCount;
                    CurrentGun.BulletCountOutGun.Value -= needBulletCount;
                }
                // 不充足
                else
                {
                    CurrentGun.BulletCountInGun.Value += CurrentGun.BulletCountOutGun.Value;
                    CurrentGun.BulletCountOutGun.Value = 0;
                }

                // 切换到正常状态
                CurrentGun.GunState.Value = GunState.Idle;
            });
        }

        private void ContinueShooting()
        {
            ITimeSystem timeSystem = this.GetSystem<ITimeSystem>();
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();

            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(CurrentGun.Name.Value);

            timeSystem.AddDelayTask(1 / gunConfigItem.Frequency, () =>
            {
                CurrentGun.GunState.Value = GunState.Idle;

                if (CurrentGun.BulletCountInGun.Value == 0 && CurrentGun.BulletCountOutGun.Value > 0)
                {
                    ReloadBullet();
                }
            });
        }
    }
}
