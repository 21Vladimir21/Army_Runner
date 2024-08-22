using System;
using System.Collections;
using _Main._Scripts.Services;
using Kimicu.YandexGames.Utils;
using SoundService.Data;
using SoundService.Scripts;
using UnityEngine;

namespace _Main._Scripts.TutorialLogic
{
    public class TutorialStepBase : MonoBehaviour
    {
        [Range(0, 5), SerializeField] protected float showDelay;
        [SerializeField] private Sound sound;


        public Action ExitCallback { get; set; }

        public virtual void Enter(Action showCallback = null)
        {
            var audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();

            if (showDelay > 0)
            {
                Coroutines.StartRoutine(ShowDelayRoutine(() =>
                {
                    gameObject.SetActive(true);
                    audioService.PlaySound(sound, volumeScale: 0.5f);
                    showCallback?.Invoke();
                }));
                return;
            }

            gameObject.SetActive(true);
            audioService.PlaySound(sound, volumeScale: 0.5f);
        }

        public virtual void Exit()
        {
            gameObject.SetActive(false);
        }

        public void Hide() => gameObject.SetActive(false);

        private IEnumerator ShowDelayRoutine(Action callback)
        {
            yield return new WaitForSeconds(showDelay);
            callback.Invoke();
        }
    }
}