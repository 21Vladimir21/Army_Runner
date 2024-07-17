using System.Collections;
using _Main._Scripts.Soilders.Bullets;
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
        
        
        

        private int _currentHealth;
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
            _canMove = false;
            animator.SetTrigger(EnemyAnimationKeys.Attach.ToString());
            StartCoroutine(PlayParticleWithDelay());

        }

        private IEnumerator PlayParticleWithDelay()
        {
            yield return new WaitForSeconds(1.5f);
            hitParticle.Play();
            OnHit.Invoke();
        }


        private void Update()
        {
            if (_canMove)
                Move();
        }

        public bool TryApplyDamage(int damage)
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
            animator.SetTrigger(EnemyAnimationKeys.Die.ToString());
            StopMove();
            OnDie.Invoke(this);
        }

        private void UpdateProgressBar()
        {
            var progressBarFillAmount = (float)_currentHealth / health;
            progressBar.value = progressBarFillAmount;
            healthText.text = $"{_currentHealth}/{health}";

        }
    }

    public enum EnemyAnimationKeys
    {
        Idle,
        Walk,
        Attach,
        Die
    }
}