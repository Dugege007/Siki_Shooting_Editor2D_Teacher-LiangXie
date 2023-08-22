using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：通用的 2D触发器 检测脚本
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public class Trigger2DCheck : MonoBehaviour
    {
        public LayerMask TargetLayers;

        public int EnterCount;

        // 引用计数器
        // 用于判断是否有物体进入触发器
        public bool IsTriggered
        {
            get
            {
                return EnterCount > 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsInLayerMask(collision.gameObject, TargetLayers))
            {
                EnterCount++;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsInLayerMask(collision.gameObject, TargetLayers))
            {
                EnterCount--;
            }
        }

        // 判断 LayerMask 中是否包含想要的 Layer
        private bool IsInLayerMask(GameObject obj,LayerMask mask)
        {
            // 根据 Layer 数值进行位移获得用于运算的 Mask 值
            int objLayerMask = 1 << obj.layer;
            return (mask.value & objLayerMask) > 0;
        }
    }
}
