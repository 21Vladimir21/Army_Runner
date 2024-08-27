using _Main._Scripts.Services;
using SoundService.Data;
using SoundService.Scripts;
using UnityEditor;
using UnityEngine;

namespace _Main._Scripts.MergeLogic
{
    public class CellToMerge : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject SelectOutLine;

        [HideInInspector] public DraggableObject currentObject;
        public bool isReserveCell;

        public bool IsBusy { get; private set; }
        private AudioService _audioService;
        private void Start() => _audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();

        private void PlayEffects()
        {
            particle.Play();
            _audioService.PlaySound(Sound.Energy);
        }

        public void AddObject(DraggableObject draggableObject, bool playParticle = false)
        {
            if (playParticle) PlayEffects();
            currentObject = draggableObject;
            draggableObject.transform.position = transform.position;
            IsBusy = true;
        }

        public void RemoveObjectData()
        {
            currentObject = null;
            IsBusy = false;
            DeSelectCell();
        }

        public void ResetSoldierPosition()
        {
            _audioService.PlaySound(Sound.PickDown,volumeScale:0.4f);
            currentObject.DownSoldier();
            IsBusy = true;
            currentObject.transform.position = transform.position;
            DeSelectCell();
        }

        public void SelectCell() => SelectOutLine.SetActive(true);

        public void DeSelectCell() => SelectOutLine.SetActive(false);

        public void StartDragObject()
        {
            _audioService.PlaySound(Sound.PickUp,volumeScale:0.4f);
            currentObject.UpSoldier();
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