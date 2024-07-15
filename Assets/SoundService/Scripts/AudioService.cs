using System;
using System.IO;
using _Main._Scripts.Services;
using SoundService.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace SoundService.Scripts
{
    public class AudioService : MonoBehaviour, IService
    {
        [SerializeField] private Sounds soundsData;
        [SerializeField] private AudioSource master;
        [SerializeField] private AudioSource music;
        [SerializeField] private AudioSource sfx;
        [field: SerializeField] public AudioMixer mixer { get; private set; }
        public bool IsCanPlaySound;

        public SoundData SoundData { get; private set; }

        public void Init(bool soundEnabled)
        {
            DontDestroyOnLoad(gameObject);
            SoundData = new SoundData(soundsData.sounds);
            IsCanPlaySound = soundEnabled;
        }

        private void Start() => SetActiveSound(IsCanPlaySound);

        public void PlaySound(Sound type, bool loop = false, float volumeScale = 1f, bool random = false)
        {
            var soundStruct = SoundData.GetSound(type);
            var clip = soundStruct.AudioClips[0];
            if (random)
            {
                var index = Random.Range(0, soundStruct.AudioClips.Count);
                clip = soundStruct.AudioClips[index];
            }

            if (soundStruct.SoundType == SoundType.SFX)
            {
                if (loop)
                {
                    sfx.clip = clip;
                    sfx.loop = true;
                    sfx.Play();
                }
                else sfx.PlayOneShot(clip, volumeScale);
            }
            else if(soundStruct.SoundType == SoundType.Music)
            {
                if (loop)
                {
                    music.clip = clip;
                    music.loop = true;
                    music.Play();
                }
                else music.PlayOneShot(clip, volumeScale);
            }
            
        }
        
        public void SetActiveSound(bool soundEnabled)
        {
            mixer.SetFloat("Volume", soundEnabled ? 0 : -80);
            IsCanPlaySound = !soundEnabled;
        }

        public void ContinueSfxSound() => sfx.UnPause();
        public void PauseSfxSound() => sfx.Pause();
        public void StopMasterSound() => master.Stop();
        public void StopSfxSound() => sfx.Stop();
#if UNITY_EDITOR


        [ContextMenu("Generate enum keys by soundType,and set names")]
        public void GenerateEnumKeys()
        {
            var fileName = "Sound";
            var path = "Assets/SoundService/Data/Sound.cs";
            using (var streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine("\tnamespace SoundService.Data\n{\n");
                streamWriter.WriteLine("\tpublic enum " + fileName);
                streamWriter.WriteLine("\t{");
                foreach (var e in soundsData.sounds)
                {
                    streamWriter.WriteLine("\t\t" + e.Sound + ", ");
                    e.name = $"{e.Sound}: Clip: {e.Clip.name}";
                }

                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}");
            }


            AssetDatabase.Refresh();
        }
#endif


        [Serializable]
        public class SoundsByType
        {
            [HideInInspector] public string name;
            public string Sound;
            public AudioClip Clip;
            public SoundType SoundType;
        }

        public enum SoundType
        {
            SFX,
            Music
        }
    }
}