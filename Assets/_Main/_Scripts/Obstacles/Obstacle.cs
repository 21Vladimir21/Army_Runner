using TMPro;
using UnityEngine;

namespace _Main._Scripts.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private TMP_Text healthText;

        private int _currentHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
            healthText.text = _currentHealth.ToString();
        }

        public bool TryApplyDamage(int damage)
        {
            if (_currentHealth < damage) return false;

            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                DestroyAnimation();
            }
            else
                healthText.text = _currentHealth.ToString();

            return true;
        }

        private void DestroyAnimation()
        {
            gameObject.SetActive(false); //TODO:Добавить анимацию падения или чего-то такого
        }
    }
}