using LocalizationSystem.Components;
using LocalizationSystem.Data.KeyGeneration;
using UnityEngine;

namespace _Main._Scripts.UI
{
    public class PhrasesHolder : MonoBehaviour
    {
        [SerializeField] private LocalizationKey[] phrases;
        [SerializeField] private LocalizationTextTMP text;

        public void RandomPhrase()
        {
            if(text == null) return;
            var randomKey = Random.Range(0, phrases.Length);
            text.TranslateByKey(phrases[randomKey]);
        }
    }
}