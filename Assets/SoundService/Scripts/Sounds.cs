using UnityEngine;

namespace SoundService.Scripts
{
    [CreateAssetMenu(fileName = "Sounds", menuName = "AudioService/New sounds data", order = 0)]
    public class Sounds : ScriptableObject
    {
        public AudioService.SoundsByType[] sounds;
    }
}