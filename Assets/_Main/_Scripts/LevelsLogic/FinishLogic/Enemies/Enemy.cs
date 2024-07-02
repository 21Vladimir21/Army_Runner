using System;
using _Main._Scripts.Soilders.Bullets;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.LevelsLogic.FinishLogic.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private int health;
        [SerializeField] private float speed;

        private int _currentHealth;
        public UnityEvent OnDie { get; private set; } = new();

        private bool _canMove;


        private void Start()
        {
            _currentHealth = health;
        }

        public void StartAttach()
        {
            _canMove = true;
            //TODO: enemy Animation
        }

        public void StopAttach()
        {
            _canMove = false;
            //TODO: enemy Animation
        }


        private void Update()
        {
            if (_canMove)
            {
                Move();
            }
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
            gameObject.SetActive(false);
            OnDie.Invoke();
            //TODO:Aнимация семрти и так далее
        }
    }
}