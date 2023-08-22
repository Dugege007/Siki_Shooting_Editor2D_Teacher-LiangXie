using FrameworkDesign;

namespace ShootingEditor2D
{
    public class KillEnemyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IStatSystem>().KillCount.Value++;
        }
    }
}
