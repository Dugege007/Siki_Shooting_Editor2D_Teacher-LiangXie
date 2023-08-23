using FrameworkDesign;
using UnityEngine;

namespace ShootingEditor2D
{
    public class KillEnemyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IStatSystem>().KillCount.Value++;

            int randomIndex = Random.Range(0, 100);
            if (randomIndex < 80)
            {
                // ������ 1 �� 3 ���ӵ�
                this.GetSystem<IGunSystem>().CurrentGun.BulletCount.Value += Random.Range(1, 4);
            }
        }
    }
}
