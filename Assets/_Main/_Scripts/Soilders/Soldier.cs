using System.Collections;
using _Main._Scripts.Boosts;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.Obstacles;
using _Main._Scripts.Soilders.Bullets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.Soilders
{
    [RequireComponent(typeof(Animator), typeof(Collider))]
    public class Soldier : MonoBehaviour,ISoldier
    {
        [HideInInspector] public UnityEvent<Soldier> onDie = new();
        [HideInInspector] public UnityEvent<Soldier> onTouchSoldier = new();
        [HideInInspector] public UnityEvent<Boost> onTouchBoost = new();
        [HideInInspector] public UnityEvent<int> onTouchMoney = new();
        [field: SerializeField] public SoldierConfig Config { get; private set; }
        [field: SerializeField] private Transform rotatableSoldier;

        [SerializeField] private Animator animator;

        [SerializeField] private Collider soldierCollider;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Transform[] doubleShootPoints;


        private float _bulletSpeed;
        private int _damage;
        private float _bulletScalePercentage = 100;
        private float _bulletLifeTime;
        private float _fireRate;

        private BulletPool _bulletPool;
        private float _timeOfLastShot;
        private bool _isDoubleShoot;

        private bool _canApplyDamage;

        public bool InCrowd { get; private set; }

        public SoldiersLevels Level => Config.SoldiersLevel;

        private SoldierAnimationTriggers _currentAnimation = SoldierAnimationTriggers.Idling;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Obstacle>())
                if (_canApplyDamage)
                    Die();

            if (other.TryGetComponent(out Soldier soldier))
            {
                if (soldier.InCrowd) return;
                onTouchSoldier.Invoke(soldier);
            }
            if (other.TryGetComponent(out Boost boost))
                onTouchBoost.Invoke(boost);

            if (other.TryGetComponent(out PickUpMoney pickUpMoney)) 
                    onTouchMoney.Invoke(pickUpMoney.Count);
        }

        public void InvitedToCrowd(BulletPool bulletPool, float damagePercentage, float speedPercentage,
            float scalePercentage,
            float fireRatePercentage)
        {
            InCrowd = true;
            _bulletPool = bulletPool;
            _bulletLifeTime = Config.bulletLifeTime;

            _fireRate = Config.fireRate;
            _damage = Config.bulletDamage;
            _bulletSpeed = Config.bulletSpeed;

            UpdateFireRatePercentage(fireRatePercentage);
            UpdateBulletDamagePercentage(damagePercentage);
            UpdateBulletSpeedPercentage(speedPercentage);
            UpdateBulletScalePercentage(scalePercentage);

            _timeOfLastShot = _fireRate;
            StartCoroutine(ApplyDamageCooldown());
        }

        public void UpdateShootingCooldown()
        {
            _timeOfLastShot += Time.deltaTime;
            if (_timeOfLastShot >= _fireRate)
            {
                Shot();
                _timeOfLastShot = 0f;
            }
        }

        public void SetLookDirection(Vector3 point)
        {
            var direction = (point - transform.position).normalized;
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.localRotation = targetRotation;
        }

        public void UpdateBulletDamagePercentage(float damageBoostPercentage) =>
            _damage = (int)(_damage * damageBoostPercentage / 100);

        public void UpdateBulletSpeedPercentage(float speedBoostPercentage) =>
            _bulletSpeed = _bulletSpeed / 100 * speedBoostPercentage;

        public void UpdateFireRatePercentage(float fireRateBoostPercentage) =>
            _fireRate *= fireRateBoostPercentage / 100;

        public void UpdateBulletScalePercentage(float scalePercentage) =>
            _bulletScalePercentage = _bulletScalePercentage / 100 * scalePercentage;

        public void ActivateDoubleShot() => _isDoubleShoot = true;

        public void SetAnimation(SoldierAnimationTriggers trigger)
        {
            if (_currentAnimation == trigger) return;
            animator.SetTrigger(trigger.ToString());
            _currentAnimation = trigger;
        }

        public void SetFinishRotation() =>
            rotatableSoldier.localRotation = Quaternion.Euler(0, Config.YFinishRotation, 0);

        private void Die()
        {
            soldierCollider.enabled = false;
            SetAnimation(SoldierAnimationTriggers.Dying);
            onDie.Invoke(this);
        }

        private void Shot()
        {
            if (_isDoubleShoot)
            {
                foreach (var point in doubleShootPoints)
                    PrepareBullet(point);
                return;
            }

            PrepareBullet(shootPoint);
        }

        private void PrepareBullet(Transform point)
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = point.position;
            bullet.transform.rotation = point.rotation;
            bullet.Shot(_bulletLifeTime, _bulletSpeed, _damage, _bulletScalePercentage);
        }

        private IEnumerator ApplyDamageCooldown()
        {
            yield return new WaitForSeconds(0.5f);
            _canApplyDamage = true;
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

    public enum SoldierAnimationTriggers
    {
        Reset,
        Idling,
        IsRunning,
        Dying,
        FinishShooting,
        Dance,
    }
}