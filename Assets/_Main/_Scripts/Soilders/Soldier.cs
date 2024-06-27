using System.Collections;
using _Main._Scripts.Obstacles;
using _Main._Scripts.Soilders.Bullets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Main._Scripts.Soilders
{
    [RequireComponent(typeof(Animator), typeof(Collider))]
    public class Soldier : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<Soldier> onDie = new();
        [field: SerializeField] public SoldierConfig Config { get; private set; }

        [SerializeField] private Animator animator;

        [SerializeField] private Collider soldierCollider;
        [SerializeField] private Transform shootPoint;

        private float _bulletSpeed;
        private int _damage;
        private float _bulletScaleRatio =1f;
        private float _bulletLifeTime;

        private BulletPool _bulletPool;
        private float _timeOfLastShot;

        public bool InCrowd { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
            {
                Die();
            }
        }

        public void InvitedToCrowd(BulletPool bulletPool, float damageRatio,float speedRatio,float scaleRatio)
        {
            InCrowd = true;
            _bulletPool = bulletPool;
            _timeOfLastShot = Config.fireRate;

            _bulletLifeTime = Config.bulletLifeTime;
            _damage = (int)(Config.bulletDamage * damageRatio);
            _bulletSpeed = Config.bulletSpeed * speedRatio;
            _bulletScaleRatio += scaleRatio;
            
        }

        public void UpdateShootingCooldown()
        {
            _timeOfLastShot += Time.deltaTime;
            if (_timeOfLastShot >= Config.fireRate)
            {
                Shot();
                _timeOfLastShot = 0f;
            }
        }

        public void UpdateBulletDamageRatio(float damageRatio) => _damage = (int)(Config.bulletDamage * damageRatio);
        public void UpdateBulletSpeedRatio(float speedRatio) => _bulletSpeed = Config.bulletSpeed * speedRatio;
        public void UpdateBulletScaleRatio(float scaleRatio) => _bulletScaleRatio += scaleRatio;

        private void Die()
        {
            soldierCollider.enabled = false;
            onDie.Invoke(this);
        }

        private void Shot()
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = shootPoint.position;
            bullet.Shot(_bulletLifeTime, _bulletSpeed, _damage,_bulletScaleRatio);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                EditorUtility.SetDirty(this);
            }

            if (soldierCollider == null)
            {
                soldierCollider = GetComponent<Collider>();
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}