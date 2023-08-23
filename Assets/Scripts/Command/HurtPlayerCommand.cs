using FrameworkDesign;

namespace ShootingEditor2D
{
    public class HurtPlayerCommand : AbstractCommand
    {
        private readonly int mHurt;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hurt">伤害量，默认值为 1</param>
        public HurtPlayerCommand(int hurt = 1)
        {
            mHurt = hurt;
        }

        protected override void OnExecute()
        {
            this.GetModel<IPlayerModel>().HP.Value -= mHurt;
        }
    }
}
