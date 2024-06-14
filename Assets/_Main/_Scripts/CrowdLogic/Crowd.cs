using System.Collections.Generic;
using _Main._Scripts.Soilders;
using UnityEngine;

namespace _Main._Scripts.CrowdLogic
{
    public class Crowd
    {
        private List<Soilder> soilders = new();

        private readonly Transform _playerTransform;

        public Crowd(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void AddToCrowd(Soilder soilder)
        {
            soilders.Add(soilder);
            soilder.onDie.AddListener(RemoveFromCrowd);
            soilder.transform.parent = _playerTransform;
        }

        private void RemoveFromCrowd(Soilder soilder)
        {
            soilders.Remove(soilder);
            soilder.transform.parent = null;
        }
    }
}