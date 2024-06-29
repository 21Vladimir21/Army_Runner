using System;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main._Scripts.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Transform obstacle;
        [SerializeField] private Transform boost;
        [SerializeField] private Collider triggerCollider;


        private int _currentHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
            healthText.text = _currentHealth.ToString();
        }

        public bool TryApplyDamage(int damage)
        {
            if (_currentHealth < 0) return false;
            
            if (_currentHealth < damage) damage = _currentHealth;
            _currentHealth -= damage;
            
            
            if (_currentHealth <= 0) DestroyAnimation();
            
            healthText.text = _currentHealth.ToString();
            return true;
        }

        private void DestroyAnimation()
        {
            triggerCollider.enabled = false;
            obstacle.DORotate(new Vector3(90, 0, 0), 0.5f).SetEase(Ease.InOutBack)
                .OnComplete(() => obstacle.gameObject.SetActive(false));
            if (boost != null) boost.DOMoveY(transform.position.y, 0.5f).SetEase(Ease.OutBounce);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (triggerCollider == null)
            {
                triggerCollider = GetComponent<Collider>();
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}