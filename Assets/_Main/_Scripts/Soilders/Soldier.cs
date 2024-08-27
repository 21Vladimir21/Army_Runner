using System;
using System.Collections;
using _Main._Scripts.Boosts;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.Obstacles;
using _Main._Scripts.Services;
using _Main._Scripts.Soilders.Bullets;
using SoundService.Data;
using SoundService.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.Soilders
{
    [RequireComponent(typeof(Animator), typeof(Collider))]
    public class Soldier : MonoBehaviour, ISoldier
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
        [SerializeField] private ParticleSystem shootParticle;
        [SerializeField] private ParticleSystem damageParticle;


        [HideInInspector] public int cellIndex = 999;

        private AudioService _audioService;

        private float _bulletSpeed;
        private float _damage;
        private float _bulletScalePercentage = 100;
        private float _bulletLifeTime;
        private float _fireRate;

        private BulletPool _bulletPool;
        private float _timeOfLastShot;
        private bool _doubleShootIsActive;

        private bool _canApplyDamage;
        private bool _canShoot = true;

        public bool InCrowd { get; private set; }

        public SoldiersLevels Level => Config.SoldiersLevel;

        private SoldierAnimationTriggers _currentAnimation = SoldierAnimationTriggers.Idling;
        private bool _isFinishShooting;
        private Quaternion _targetRotation;

        private void Start() => _audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Obstacle>())
                if (_canApplyDamage)
                {
                    damageParticle.Play();
                    _audioService.PlaySound(Sound.Poof, volumeScale: 0.25f);
                    Die();
                }

            if (other.TryGetComponent(out Soldier soldier))
            {
                if (soldier.InCrowd) return;
                onTouchSoldier.Invoke(soldier);
            }

            if (other.TryGetComponent(out Boost boost))
            {
                onTouchBoost.Invoke(boost);
                _audioService.PlaySound(Sound.Energy, volumeScale: 0.7f);
                boost.Take();
            }

            if (other.TryGetComponent(out PickUpMoney pickUpMoney))
            {
                onTouchMoney.Invoke(pickUpMoney.Count);
                pickUpMoney.TakeMoney();
            }
        }

        public void InvitedToCrowd(BulletPool bulletPool, float damagePercentage,
            float speedPercentage, float scalePercentage, float fireRatePercentage, bool doubleShootIsActive)
        {
            InCrowd = true;
            _isFinishShooting = false;
            _canApplyDamage = false;
            _bulletPool = bulletPool;
            _bulletLifeTime = Config.bulletLifeTime;

            _fireRate = Config.fireRate;
            _damage = Config.bulletDamage;
            _bulletSpeed = Config.bulletSpeed;
            soldierCollider.enabled = true;

            UpdateFireRatePercentage(fireRatePercentage);
            UpdateBulletDamagePercentage(damagePercentage);
            UpdateBulletSpeedPercentage(speedPercentage);
            UpdateBulletScalePercentage(scalePercentage);
            _doubleShootIsActive = doubleShootIsActive;
            _timeOfLastShot = _fireRate;
            StartCoroutine(WaitCooldown(0.5f, () => _canApplyDamage = true));
            gameObject.layer = LayerMask.NameToLayer(SoldierLayers.NonInteract.ToString());
        }

        private void OnDisable()
        {
            rotatableSoldier.localRotation = Quaternion.Euler(Vector3.zero);
            cellIndex = 999;
            _bulletScalePercentage = 100;
            _timeOfLastShot = 0f;
            _canShoot = true;
            gameObject.layer = LayerMask.NameToLayer(SoldierLayers.Interact.ToString());
            _currentAnimation = SoldierAnimationTriggers.Idling;
        }

        public void UpdateShootingCooldown()
        {
            if (_canShoot == false) return;
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
            _targetRotation = Quaternion.LookRotation(direction);
            transform.localRotation = _targetRotation;
        }

        public void UpdateBulletDamagePercentage(float damageBoostPercentage) =>
            _damage = _damage * damageBoostPercentage / 100;

        public void UpdateBulletSpeedPercentage(float speedBoostPercentage) =>
            _bulletSpeed = _bulletSpeed / 100 * speedBoostPercentage;

        public void UpdateFireRatePercentage(float fireRateBoostPercentage) =>
            _fireRate *= fireRateBoostPercentage / 100;

        public void UpdateBulletScalePercentage(float scalePercentage) =>
            _bulletScalePercentage = _bulletScalePercentage / 100 * scalePercentage;

        public void ActivateDoubleShot() => _doubleShootIsActive = true;

        public void SetAnimation(SoldierAnimationTriggers trigger, bool setToForce = false)
        {
            animator.ResetTrigger(_currentAnimation.ToString());
            if ((setToForce == false && _currentAnimation == trigger) ||
                _currentAnimation == SoldierAnimationTriggers.Dance) return;
            animator.SetTrigger(trigger.ToString());
            _currentAnimation = trigger;
        }

        public void SetFinishRotation() =>
            rotatableSoldier.localRotation = Quaternion.Euler(0, Config.YFinishRotation, 0);

        public void EnableFinishShooting() => _isFinishShooting = true;

        private void Die()
        {
            soldierCollider.enabled = false;
            SetAnimation(SoldierAnimationTriggers.Dying);
            onDie.Invoke(this);
        }

        private void Shot()
        {
            _canShoot = false;
            if (_doubleShootIsActive && _isFinishShooting == false)
            {
                foreach (var point in doubleShootPoints)
                    StartCoroutine(ShootingWithDelay(point));
                return;
            }

            StartCoroutine(ShootingWithDelay(shootPoint));
        }

        private IEnumerator ShootingWithDelay(Transform point)
        {
            for (int i = 0; i < Config.oneShotBulletCount; i++)
            {
                PrepareBullet(point);
                _audioService.PlaySound(Sound.Shot, volumeScale: 0.1f);
                shootParticle.Play();
                if (_isFinishShooting)
                    SetAnimation(SoldierAnimationTriggers.Shot, true);
                yield return new WaitForSeconds(Config.bulletShotDelay);
            }

            _canShoot = true;
        }

        private void PrepareBullet(Transform point)
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = point.position;
            bullet.transform.rotation = point.rotation;
            bullet.Shot(_bulletLifeTime, _bulletSpeed, _damage, _bulletScalePercentage);
        }

        private IEnumerator WaitCooldown(float waitTime, Action callback)
        {
            yield return new WaitForSeconds(waitTime);
            callback.Invoke();
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
        Shot,
    }

    public enum SoldierLayers
    {
        Interact,
        NonInteract
    }
}