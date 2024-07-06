using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.MergeLogic
{
    public class CellToMerge : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnReturnObject = new();
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
    }
}