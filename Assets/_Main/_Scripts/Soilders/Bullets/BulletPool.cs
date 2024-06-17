using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.Soilders
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int startObjectCount;

        private List<Bullet> _bullets = new();

        private void Start()
        {
            for (int i = 0; i < startObjectCount; i++)
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
            bullet.transform.position = transform.position;
        }

        private Bullet AddBullet()
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.OnLifeTimeEnded.AddListener(ReturnBullet);
            _bullets.Add(bullet);

            return bullet;
        }
    }
}