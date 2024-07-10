using System;
using System.Collections;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using UnityEngine;

namespace _Main._Scripts.UI
{
    public abstract class AbstractView : MonoBehaviour
    {
        protected Saves Saves;

        public void Close(Action callback = null)
        {
            gameObject.SetActive(false);
            callback?.Invoke();
        }

        public void Open(Action callback = null)
        {
            gameObject.SetActive(true);
            callback?.Invoke();
        }

        public virtual void Init()
        {
            var savesService = ServiceLocator.Instance.GetServiceByType<SavesService>();
            Saves = savesService.Saves;
        }

        protected IEnumerator ShowWithDelay(Action callback, float delay = 0.5f)
        {
            yield return new WaitForSeconds(delay);
            callback.Invoke();
        }
    }
}