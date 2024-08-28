using _Main._Scripts.LevelsLogic.FinishLogic.Enemies;
using _Main._Scripts.Soilders.Bullets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.Obstacles
{
    public class EnemyObstacle : MonoBehaviour, IDamageable
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Collider enemyCollider;
        [SerializeField] private Animator animator;
        [Space] [SerializeField] private int health;
        [Range(0, 100), SerializeField] private float speed;

        private float _currentHealth;
        private bool _canMove;

        private void Start()
        {
            StartMove();
            _currentHealth = health;
            UpdateProgressBar();
        }

        private void StartMove()
        {
            enemyCollider.enabled = true;
            _canMove = true;
            animator.SetTrigger(EnemyAnimationKeys.Walk.ToString());
        }

        private void StopMove()
        {
            enemyCollider.enabled = false;
            _canMove = false;
            animator.SetTrigger(EnemyAnimationKeys.Idle.ToString());
        }

        public bool TryApplyDamage(float damage)
        {
            if (_currentHealth < 0) return false;

            if (_currentHealth < damage) damage = _currentHealth;
            _currentHealth -= damage;
            UpdateProgressBar();


            if (_currentHealth <= 0) Die();

            var currentHealthString =
                _currentHealth % 1 == 0 ? _currentHealth.ToString("F0") : _currentHealth.ToString("F1");
            healthText.text = $"{currentHealthString}/{health}";
            return true;
        }

        private void Update()
        {
            if (_canMove)
                Move();
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
}