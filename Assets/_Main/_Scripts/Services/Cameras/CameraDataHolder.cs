using System;
using Cinemachine;
using UnityEditor;
using UnityEngine;

namespace _Main._Scripts.Services.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraDataHolder : MonoBehaviour
    {
        [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        [field: SerializeField] public CameraType Type { get; private set; }

        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);
#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (VirtualCamera)
                return;
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}