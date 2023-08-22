using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：枪
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public class Gun : MonoBehaviour
    {
        private Bullet mBullet;

        private void Awake()
        {
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();
        }

        public void Shoot()
        {
            var bullet = Instantiate(mBullet, mBullet.transform.position, mBullet.transform.rotation);
            // 将全局的缩放值设置给子弹
            bullet.transform.localScale = mBullet.transform.lossyScale;

            bullet.gameObject.SetActive(true);
        }
    }
}
