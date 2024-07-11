using System;
using _Main._Scripts.CrowdLogic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _Main._Scripts.MergeLogic
{
    public class DraggableObject : MonoBehaviour, ISoldier
    {
        [field: SerializeField] public SoldiersLevels Level { get; private set; }
        [SerializeField] private Animator animator;
        
        private const string IdleKey = "Idling";
        private const string HangingKey = "Hanging";
        private bool _isHinging;


        public void UpSoldier()
        {
            if (_isHinging) return;
            _isHinging = true;
            animator.SetTrigger(HangingKey);
        }

        public void DownSoldier()
        {
            if (_isHinging == false) return;
            _isHinging = false;
            animator.SetTrigger(IdleKey);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animator != null) return;
            animator = GetComponent<Animator>();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}