using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.MergeLogic
{
    public class CellToMerge : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnDestroyObject = new();
        [HideInInspector] public DraggableObject currentObject;
        [field:SerializeField]public bool IsBusy { get; private set; } 

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

        public void ResetCurrentObjectPosition()
        {
            IsBusy = true;
            currentObject.transform.position = transform.position;
        }

        public void DestroyObject()
        {
            Destroy(currentObject.gameObject);
            OnDestroyObject.Invoke();
            RemoveObject();
        }
    }
}