using System;
using System.Collections;
using Kimicu.YandexGames.Utils;
using UnityEngine;

namespace _Main._Scripts.TutorialLogic
{
    public class TutorialStepBase : MonoBehaviour
    {
        [Range(0, 5), SerializeField] protected float showDelay;
        
        public Action ExitCallback { get; set; }

        public virtual void Enter(Action showCallback = null)
        {
            if (showDelay > 0)
            {
                Coroutines.StartRoutine(ShowDelayRoutine(() =>
                {
                    gameObject.SetActive(true);
                    showCallback?.Invoke();
                }));
                return;
            }
            gameObject.SetActive(true);
        }

        public virtual void Exit()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator ShowDelayRoutine(Action callback)
        {
            yield return new WaitForSeconds(showDelay);
            callback.Invoke();
        }
    }
}