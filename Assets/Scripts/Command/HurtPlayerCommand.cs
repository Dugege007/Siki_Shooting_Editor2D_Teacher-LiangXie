using QFramework;
using UnityEngine.SceneManagement;

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
            IPlayerModel playerModel = this.GetModel<IPlayerModel>();
            playerModel.HP.Value -= mHurt;

            if (playerModel.HP.Value <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
