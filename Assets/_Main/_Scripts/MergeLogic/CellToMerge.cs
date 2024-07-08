using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.MergeLogic
{
    public class CellToMerge : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        [HideInInspector] public UnityEvent OnReturnObject = new();
        [HideInInspector] public DraggableObject currentObject;
        public bool IsBusy { get; private set; }

        public void PlaySpawnParticle() => particle.Play();

        public void AddObject(DraggableObject draggableObject)
        {
            currentObject = draggableObject;
            draggableObject.transform.position = transform.position;
            IsBusy = true;
        }

        public void RemoveObject()
        {
            currentObject = null;
            IsBusy = false;
        }

        public void StartDragObject() => currentObject.UpSoldier();

        public void ResetCurrentObject()
        {
            currentObject.DownSoldier();
            IsBusy = true;
            currentObject.transform.position = transform.position;
        }

        public void ReturnObject()
        {
            OnReturnObject.Invoke();
            RemoveObject();
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