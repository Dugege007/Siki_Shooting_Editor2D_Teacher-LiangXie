using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public abstract class ShootingEditor2DController : MonoBehaviour, IController
    {
        // 接口阉割
        // 用 IBelongToArchitecture 接口限制访问
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }
    }
}
