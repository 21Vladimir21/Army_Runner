using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace _Main._Scripts.Services.Cameras
{
    [Serializable]
    public class CamerasHolder
    {
        private Camera _mainCamera;
        [field: SerializeField] public CinemachineBrain Brain { get; private set; }
        public Camera MainCamera
        {
            get
            {
                if (_mainCamera) return _mainCamera;
                Brain.TryGetComponent(out Camera camera);
                _mainCamera = camera;
                return camera;
            }
        }

        [field: SerializeField] public List<CameraDataHolder> Cameras { get; private set; }
    }
}