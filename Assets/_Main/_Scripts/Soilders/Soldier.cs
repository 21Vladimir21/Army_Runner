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
        [field:SerializeField] public SoldierConfig Config { get; private set; }

        [SerializeField] private Animator animator;

        [SerializeField] private Collider soldierCollider;
        [SerializeField] private Transform shootPoint;


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


        public void InvitedToCrowd(BulletPool bulletPool)
        {
            InCrowd = true;
            _bulletPool = bulletPool;
            _timeOfLastShot = Config.fireRate;
        }

        private void Die()
        {
            soldierCollider.enabled = false;
            onDie.Invoke(this);
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

        private void Shot()
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = shootPoint.position;
            bullet.Shot(Config.bulletLifeTime, Config.bulletSpeed);
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