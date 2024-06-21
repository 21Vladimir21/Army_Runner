using Cinemachine;
using UnityEngine;

namespace _Main._Scripts.Services.Camera
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private CameraHolder holder;

        private CinemachineVirtualCamera _currentCamera;
        private void Awake() => DontDestroyOnLoad(this);

        public void SwitchTo(CinemachineVirtualCamera targetCam)
        {
            _currentCamera.gameObject.SetActive(false);
            targetCam.gameObject.SetActive(true);
            _currentCamera = targetCam;
        }
    }
}