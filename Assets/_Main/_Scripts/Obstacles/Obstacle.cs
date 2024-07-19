using _Main._Scripts.Soilders.Bullets;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _Main._Scripts.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour,IDamageable
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Transform obstacle;
        [SerializeField] private DescendingObject objectForDown;
        [SerializeField] private Collider triggerCollider;
        [SerializeField] private ParticleSystem dustParticle;
        [SerializeField] private bool dontDamagable;
        
        


        private float _currentHealth;

        private void Start()
        {


            if (dontDamagable) return;
            _currentHealth = maxHealth;
            healthText.text = _currentHealth.ToString();
            if (objectForDown != null) objectForDown.DeactivateTrigger();
        }

        public bool TryApplyDamage(float damage)
        {
            if (dontDamagable) return false;
            if (_currentHealth < 0) return false;
            
            if (_currentHealth < damage) damage = _currentHealth;
            _currentHealth -= damage;


            if (_currentHealth <= 0)
            {
                dustParticle.Play();
                DestroyAnimation();
            }
            
            healthText.text = _currentHealth.ToString("F2");
            return true;
        }

        private void DestroyAnimation()
        {
            triggerCollider.enabled = false;
            obstacle.DORotate(new Vector3(90, 0, 0), 0.5f).SetEase(Ease.InOutBack)
                .OnComplete(() => obstacle.gameObject.SetActive(false));
            if (objectForDown != null)
            {
                objectForDown.ActivateTrigger();
                objectForDown.transform.DOMoveY(transform.position.y, 0.5f).SetEase(Ease.OutBounce);
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (triggerCollider == null)
            {
                triggerCollider = GetComponent<Collider>();
                EditorUtility.SetDirty(this);
            }

            if(dontDamagable)
                return;
            healthText.text = maxHealth.ToString();
        }
#endif
    }
}