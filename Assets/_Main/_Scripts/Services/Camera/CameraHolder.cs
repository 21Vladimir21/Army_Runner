using System;
using Cinemachine;
using UnityEngine;

namespace _Main._Scripts.Services.Camera
{
    [Serializable]
    public class CameraHolder
    {
        [field: SerializeField] public CinemachineBrain Brain { get; private set; }
        [field: Header("Camera group _ 1")]
        [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }
    }
}