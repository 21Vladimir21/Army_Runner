using _Main._Scripts.Soilders.Bullets;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.LevelsLogic.FinishLogic.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private int health;
        [SerializeField] private float speed;

        private int _currentHealth;
        public UnityEvent OnDie { get; private set; } = new();

        private bool _canMove;


        private void Start() => _currentHealth = health;

        public void StartMove()
        {
            _canMove = true;
            animator.SetTrigger(EnemyAnimationKeys.Walk.ToString());
        }

        public void StopMove()
        {
            _canMove = false;
            animator.SetTrigger(EnemyAnimationKeys.Idle.ToString());
        }

        public void Attach()
        {
            _canMove = false;
            animator.SetTrigger(EnemyAnimationKeys.Attach.ToString());
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
            OnDie.Invoke();
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