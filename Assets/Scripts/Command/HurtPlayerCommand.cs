using QFramework;
using UnityEngine.SceneManagement;

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
            IPlayerModel playerModel = this.GetModel<IPlayerModel>();
            playerModel.HP.Value -= mHurt;

            if (playerModel.HP.Value <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
