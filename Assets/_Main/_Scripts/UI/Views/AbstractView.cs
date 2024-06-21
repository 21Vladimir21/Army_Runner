using System;
using UnityEngine;

namespace _Main._Scripts.UI
{
    public abstract class AbstractView : MonoBehaviour 
    {
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

        public void Init()
        {
        }
    }
}