using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.Soilders.Bullets
{
    public class BulletPool
    {
         private Bullet _bulletPrefab;
         private int _startBulletCount;

        private List<Bullet> _bullets = new();

        public BulletPool(BulletPoolConfig config)
        {
            _bulletPrefab = config.BulletPrefab;
            _startBulletCount = config.StartBulletCount;
            for (int i = 0; i < _startBulletCount; i++)
            {
                var bullet = AddBullet();
                bullet.gameObject.SetActive(false);
            }
        }

        public Bullet GetBullet()
        {
            foreach (var bullet in _bullets)
            {
                if (bullet.gameObject.activeSelf == false)
                {
                    bullet.gameObject.SetActive(true);
                    return bullet;
                }
            }

            return AddBullet();
        }

        private void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            bullet.transform.position = Vector3.zero;
        }

        private Bullet AddBullet()
        {
            var bullet = Object.Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity);
            bullet.OnLifeTimeEnded.AddListener(ReturnBullet);
            _bullets.Add(bullet);

            return bullet;
        }
    }
}