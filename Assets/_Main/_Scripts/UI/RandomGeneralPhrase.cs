using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main._Scripts.UI
{
    [Serializable]
    public class RandomGeneralPhrase
    {
        [SerializeField] private PhrasesHolder[] phrases = new PhrasesHolder[]{};

        public void ShowRandomPhrase()
        {
            foreach (var phrasesHolder in phrases) phrasesHolder.gameObject.SetActive(false);
            var phrase = phrases[Random.Range(0, phrases.Length)];
            phrase.gameObject.SetActive(true);
            phrase.RandomPhrase();
        }
    }
}