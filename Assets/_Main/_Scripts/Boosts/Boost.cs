using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Main._Scripts.Boosts
{
    public class Boost : MonoBehaviour
    {
        [field: SerializeField] public BoostType Type { get; private set; }
        [SerializeField] private TMP_Text percentageText;
        [field:Range(-100,100), SerializeField] public float Value { get; private set; }
        [SerializeField] public ParticleSystem particle;
        [SerializeField] public Collider triggerCollider;

        private void Start()
        {
            if (percentageText != null)
                percentageText.text = Value + "%";
        }

        public void Take()
        {
            particle.Play();
            triggerCollider.enabled = false;
            transform.DORotate(new Vector3(90, 0, 0), 0.5f).SetEase(Ease.InOutBack)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}