using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main._Scripts.UI
{
    [Serializable]
    public class RandomGeneralPhrase
    {
        [SerializeField] private GameObject[] phrases;

        public void ShowRandomPhrase()
        {
            foreach (var gameObject in phrases) gameObject.SetActive(false);
            var phrase = phrases[Random.Range(0, phrases.Length)];
            phrase.SetActive(true);
        }
    }
}