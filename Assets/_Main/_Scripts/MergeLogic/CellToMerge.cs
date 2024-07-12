using System;
using _Main._Scripts.Services;
using SoundService.Data;
using SoundService.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.MergeLogic
{
    public class CellToMerge : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject SelectOutLine;

        [HideInInspector] public UnityEvent OnReturnObject = new();
        [HideInInspector] public DraggableObject currentObject;

        public bool IsBusy { get; private set; }
        private AudioService _audioService;
        private void Start() => _audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();

        public void PlaySpawnParticle()
        {
            particle.Play();
            _audioService.PlaySound(Sound.Energy);
        }

        public void AddObject(DraggableObject draggableObject,bool playParticle = false)
        {
            if (playParticle) PlaySpawnParticle();
            currentObject = draggableObject;
            draggableObject.transform.position = transform.position;
            IsBusy = true;
        }

        public void RemoveObject()
        {
            currentObject = null;
            IsBusy = false;
            DeSelectCell(); 
        }

        public void SelectCell() => SelectOutLine.SetActive(true);
        public void DeSelectCell() => SelectOutLine.SetActive(false);

        public void StartDragObject()
        {
            currentObject.UpSoldier();
        }

        public void ResetCurrentObject()
        {
            currentObject.DownSoldier();
            IsBusy = true;
            currentObject.transform.position = transform.position;
            DeSelectCell();
        }

        public void ReturnObject()
        {
            OnReturnObject.Invoke();
            RemoveObject();
            DeSelectCell();
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (particle != null) return;
            particle = GetComponentInChildren<ParticleSystem>();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}