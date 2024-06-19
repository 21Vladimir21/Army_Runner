using System.Collections;
using _Main._Scripts.Obstacles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.Soilders
{
    [RequireComponent(typeof(Animator), typeof(Collider))]
    public class Soldier : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<Soldier> onDie = new();
        [SerializeField] private SoldierConfig config;
        
        [SerializeField] private Animator animator;
        [SerializeField] private Collider collider;
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
            _timeOfLastShot = config.fireRate;
        }

        private void Die()
        {
            collider.enabled = false;
            onDie.Invoke(this);
        }

        public void UpdateShootingCooldown()
        {
            _timeOfLastShot += Time.deltaTime;
            if (_timeOfLastShot >= config.fireRate)
            {
                Shot();
                _timeOfLastShot = 0f;
            }
        }

        private void Shot()
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = shootPoint.position;
            bullet.Shot(config.bulletLifeTime, config.bulletSpeed);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                EditorUtility.SetDirty(this);
            }

            if (collider == null)
            {
                collider = GetComponent<Collider>();
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}