using System;
using UnityEngine;

namespace _Main._Scripts.Obstacles
{
    public abstract class DescendingObject : MonoBehaviour
    {
        [SerializeField] protected Collider triggerCollider;


        public void DeactivateTrigger() => triggerCollider.enabled = false;
        public void ActivateTrigger() => triggerCollider.enabled = true;
    }
}