using UnityEngine;

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
