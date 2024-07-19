using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.Services.Cameras
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private Image fade;
        [field: SerializeField] public CamerasHolder Holder { get; private set; }
        private bool _isFaded = true;


        private CameraDataHolder _currentCamera;
        private Coroutine _currentBlendRoutine;


        public void SwitchToFromType(CameraType type, Transform newPosition = null)
        {
            if (_currentBlendRoutine != null) StopCoroutine(_currentBlendRoutine);
            var cameraData = Holder.Cameras.FirstOrDefault(x => x.Type == type);
            if (cameraData != null)
            {
                if (_currentCamera) _currentCamera.Disable();
                _currentCamera = cameraData;
                if (newPosition != null)
                {
                    _currentCamera.VirtualCamera.transform.position = newPosition.position;
                    _currentCamera.VirtualCamera.transform.rotation = newPosition.rotation;
                }

                _currentCamera.Enable();
            }
            else Debug.LogWarning($"Camera {type} not found");
        }
        

        public void ShowFade(Action endCallback = null)
        {
            if (_isFaded)
            {
                endCallback?.Invoke();
                return;
            }

            fade.DOFade(1, 0.5f)
                .OnComplete(() =>
                {
                    _isFaded = true;
                    endCallback?.Invoke();
                });
        }

        public void HideFade(Action endCallback = null, bool forceCallback = false)
        {
            if (_isFaded == false || forceCallback)
            {
                endCallback?.Invoke();
                _isFaded = false;
                return;
            }

            fade.DOFade(0, 0.5f)
                .OnComplete(() =>
                {
                    _isFaded = false;
                    endCallback?.Invoke();
                });
        }
    }
}