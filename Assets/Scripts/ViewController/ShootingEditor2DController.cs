using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public abstract class ShootingEditor2DController : MonoBehaviour, IController
    {
        // �ӿ��˸�
        // �� IBelongToArchitecture �ӿ����Ʒ���
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }
    }
}
