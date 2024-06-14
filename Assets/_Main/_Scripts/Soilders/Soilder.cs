using System;
using _Main._Scripts.Obstacles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.Soilders
{
    [RequireComponent(typeof(Animator), typeof(Collider))]
    public class Soilder : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Collider collider;

        public UnityEvent<Soilder> onDie = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
            {
                Die();
            }
        }

        private void Die()
        {
            collider.enabled = false;
            onDie.Invoke(this);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                EditorUtility.SetDirty(this);
            }

            if (collider == null)
            {
                collider = GetComponent<Collider>();
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}