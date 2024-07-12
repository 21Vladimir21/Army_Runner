using _Main._Scripts.Obstacles;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Main._Scripts.Boosts
{
    public class Boost : DescendingObject
    {
        [field: SerializeField] public BoostType Type { get; private set; }
        [SerializeField] private TMP_Text percentageText;
        [field:Range(-100,100), SerializeField] public float Value { get; private set; }
        [SerializeField] public ParticleSystem particle;

        private void Start()
        {
            if (percentageText != null)
                percentageText.text = Value + "%";
        }

        public void Take()
        {
            particle.Play();
           DeactivateTrigger();
            transform.DORotate(new Vector3(90, 0, 0), 0.5f).SetEase(Ease.InOutBack)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}