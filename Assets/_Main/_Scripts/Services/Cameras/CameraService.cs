using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace _Main._Scripts.Services.Cameras
{
    public class CameraService : MonoBehaviour
    {
        [field: SerializeField] public CamerasHolder Holder { get; private set; }


        private CameraDataHolder _currentCamera;

        private void Awake()
        {
            // DontDestroyOnLoad(this);
        }

        public void SwitchToFromType(CameraType type, Action openCallback = null, Action closeCallBack = null)
        {
            var cameraData = Holder.Cameras.FirstOrDefault(x => x.Type == type);
            if (cameraData != null)
            {
                if (_currentCamera) _currentCamera.Disable(closeCallBack);
                _currentCamera = cameraData;
                _currentCamera.Enable(openCallback);
            }
            else Debug.LogWarning($"Camera {type} not found");
        }

        public void SwitchTo(CinemachineVirtualCamera targetCam)
        {
            _currentCamera.gameObject.SetActive(false);
            targetCam.gameObject.SetActive(true);
            // _currentCamera = targetCam;
        }
    }
}