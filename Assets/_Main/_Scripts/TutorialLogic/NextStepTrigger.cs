using System;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.Services;
using UnityEngine;

namespace _Main._Scripts.TutorialLogic
{
    [RequireComponent(typeof(Collider))]
    public class NextStepTrigger : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private GameObject mask;

        private TutorialService _tutorialService;
        private void Start() => _tutorialService = ServiceLocator.Instance.GetServiceByType<TutorialService>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _tutorialService.TryCallNextStep();
                player.MouseInput = false;
                mask.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (mask == null) return;
            if (other.TryGetComponent(out Player player)) mask.SetActive(false);
            collider.enabled = false;
        }
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (collider == null) collider = GetComponent<Collider>();
        }
#endif
    }
}