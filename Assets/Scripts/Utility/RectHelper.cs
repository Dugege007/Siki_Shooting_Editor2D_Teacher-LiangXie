using UnityEngine;

// 工具层中的代码不必都继承自本框架，也可以是静态类，也可以是其他框架的工具
// 主要为其他三层提供一些支撑，提高效率

namespace ShootingEditor2D
{
    public static class RectHelper
    {
        /// <summary>
        /// 创建一个矩形（给定位置和大小）
        /// </summary>
        /// <param name="x">中心点的X坐标</param>
        /// <param name="y">中心点的Y坐标</param>
        /// <param name="width">矩形的宽度</param>
        /// <param name="height">矩形的高度</param>
        /// <returns>表示矩形的Rect对象</returns>
        public static Rect RectForAnchorCenter(float x, float y, float width, float height)
        {
            float finalX = x - width * 0.5f;
            float finalY = y - height * 0.5f;

            return new Rect(finalX, finalY, width, height);
        }

        /// <summary>
        /// 创建一个矩形（给定位置和大小）
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="size">大小</param>
        /// <returns>表示矩形的Rect对象</returns>
        public static Rect RectForAnchorCenter(Vector2 pos, Vector2 size)
        {
            float finalX = pos.x - size.x * 0.5f;
            float finalY = pos.y - size.y * 0.5f;

            return new Rect(finalX, finalY, size.x, size.y);
        }
    }
}
