using System;
using System.Collections;
using _Main._Scripts.Services;
using _Main._Scripts.Soilders.Bullets;
using SoundService.Data;
using SoundService.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Main._Scripts.LevelsLogic.FinishLogic.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private int health;
        [SerializeField] private float speed;
        [SerializeField] private Collider enemyCollider;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private ParticleSystem hitParticle;


        private float _currentHealth;
        public UnityEvent<Enemy> OnDie { get; private set; } = new();
        public UnityEvent OnHit { get; private set; } = new();

        private bool _canMove;


        private void Start()
        {
            _currentHealth = health;
            UpdateProgressBar();
        }

        public void StartMove()
        {
            enemyCollider.enabled = true;
            _canMove = true;
            animator.SetTrigger(EnemyAnimationKeys.Walk.ToString());
        }

        public void StopMove()
        {
            enemyCollider.enabled = false;
            _canMove = false;
            animator.SetTrigger(EnemyAnimationKeys.Idle.ToString());
        }

        public void Attach()
        {
            var audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();

            _canMove = false;
            animator.SetTrigger(EnemyAnimationKeys.Attach.ToString());
            StartCoroutine(PlayParticleWithDelay(() => audioService.PlaySound(Sound.EnemyHit, volumeScale: 0.3f)));
        }

        private IEnumerator PlayParticleWithDelay(Action callback)
        {
            yield return new WaitForSeconds(1.5f);
            hitParticle.Play();
            OnHit.Invoke();
            callback?.Invoke();
        }


        private void Update()
        {
            if (_canMove)
                Move();
        }

        public bool TryApplyDamage(float damage)
        {
            if (_currentHealth < 0) return false;

            if (_currentHealth < damage) damage = _currentHealth;
            _currentHealth -= damage;
            UpdateProgressBar();

            if (_currentHealth <= 0) Die();

            return true;
        }

        private void Move()
        {
            var forwardDirection = transform.forward;
            var moveDirection = forwardDirection.normalized * speed;
            transform.position += moveDirection * Time.deltaTime;
        }

        private void Die()
        {
            progressBar.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            animator.SetTrigger(EnemyAnimationKeys.Die.ToString());
            StopMove();
            OnDie.Invoke(this);
        }

        private void UpdateProgressBar()
        {
            var progressBarFillAmount = _currentHealth / health;
            progressBar.value = progressBarFillAmount;
            var currentHealthString =
                _currentHealth % 1 == 0 ? _currentHealth.ToString("F0") : _currentHealth.ToString("F1");
            healthText.text = $"{currentHealthString}/{health}";
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            healthText.text = health.ToString();
        }
#endif
    }

    public enum EnemyAnimationKeys
    {
        Idle,
        Walk,
        Attach,
        Die
    }
}