using System;
using System.Collections.Generic;
using SoundService.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoundService.Scripts
{
    public class SoundData
    {
        private readonly Dictionary<SoundType, List<AudioClip>> _soundDict;

        public SoundData(IEnumerable<AudioService.SoundsByType> sounds)
        {
            _soundDict = new Dictionary<SoundType, List<AudioClip>>();

            foreach (var sound in sounds)
            {
                var soundType = (SoundType)Enum.Parse(typeof(SoundType), sound.SoundType);
                if (!_soundDict.ContainsKey(soundType))
                    _soundDict.Add(soundType, new List<AudioClip> { sound.Clip });
                else _soundDict[soundType].Add(sound.Clip);
            }
        }

        public AudioClip GetSound(SoundType type, bool random = false)
        {
            if (_soundDict.TryGetValue(type, out var value))
            {
                if (!random) return value[0];

                var index = Random.Range(0, value.Count);
                return value[index];
            }

            Debug.Log($"The sound with type {type} not founded!");
            return null;
        }
    }
}