using System;
using System.Collections.Generic;
using SoundService.Data;
using UnityEngine;

namespace SoundService.Scripts
{
    public class SoundData
    {
        private readonly Dictionary<Sound, SoundStruct> _soundDict;

        public SoundData(IEnumerable<AudioService.SoundsByType> sounds)
        {
            _soundDict = new Dictionary<Sound, SoundStruct>();

            foreach (var sound in sounds)
            {
                var soundType = (Sound)Enum.Parse(typeof(Sound), sound.Sound);
                if (!_soundDict.ContainsKey(soundType))
                    _soundDict.Add(soundType, new SoundStruct(new List<AudioClip> { sound.Clip },sound.SoundType));
                else _soundDict[soundType].AudioClips.Add(sound.Clip);
            }
        }

        public SoundStruct GetSound(Sound type)
        {
            if (_soundDict.TryGetValue(type, out var value))
                return value;

            Debug.Log($"The sound with type {type} not founded!");
            return default;
        }
    }

    [Serializable]
    public struct SoundStruct
    {
        public List<AudioClip> AudioClips;
        public AudioService.SoundType SoundType;

        public SoundStruct(List<AudioClip> audioClips, AudioService.SoundType soundType)
        {
            AudioClips = audioClips;
            SoundType = soundType;
        }
    }
}