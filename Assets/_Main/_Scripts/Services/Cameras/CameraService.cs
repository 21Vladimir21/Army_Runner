using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace _Main._Scripts.Services.Cameras
{
    public class CameraService : MonoBehaviour
    {
        [field: SerializeField] public CamerasHolder Holder { get; private set; }


        private CameraDataHolder _currentCamera;
        

        public void SwitchToFromType(CameraType type, Action blendCompletedCallBack = null, Action closeCallBack = null)
        {
            var cameraData = Holder.Cameras.FirstOrDefault(x => x.Type == type);
            if (cameraData != null)
            {
                if (_currentCamera) _currentCamera.Disable();
                _currentCamera = cameraData;
                _currentCamera.Enable();
                if (blendCompletedCallBack != null) 
                    StartCoroutine(WaitBlendCompleted(blendCompletedCallBack));
            }
            else Debug.LogWarning($"Camera {type} not found");
        }

        private IEnumerator WaitBlendCompleted(Action callback)
        {
            yield return new WaitUntil(() => Holder.Brain.IsBlending);
            yield return new WaitForSeconds(Holder.Brain.ActiveBlend.Duration);
            callback.Invoke();
        }
        
    }
}