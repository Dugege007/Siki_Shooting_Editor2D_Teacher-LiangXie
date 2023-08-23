using FrameworkDesign;

namespace ShootingEditor2D
{
    public class HurtPlayerCommand : AbstractCommand
    {
        private readonly int mHurt;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="hurt">�˺�����Ĭ��ֵΪ 1</param>
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
