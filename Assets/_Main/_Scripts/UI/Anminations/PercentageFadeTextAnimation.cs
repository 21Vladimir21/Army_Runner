using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Main._Scripts.UI.Anminations
{
    public class PercentageFadeTextAnimation : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private float plusYValue;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;

        private Vector3 _defaultPosition;
        private Sequence _sequence;
        private float _currentPercentage;


        private void Start()
        {
            _defaultPosition = text.transform.localPosition;
            _sequence = DOTween.Sequence();
        }

        public void SetText(float value) => _currentPercentage = Mathf.Abs(value);

        public void StartAnimation(float value = 0)
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            text.text = "+" + _currentPercentage + "%";
            _currentPercentage = Mathf.Abs(value);
            text.transform.localPosition = _defaultPosition;
            text.alpha = 1;
            _sequence.Append(
                    text.transform.DOLocalMoveY(transform.localPosition.y + plusYValue, duration).SetEase(ease))
                .Insert(0, text.DOFade(0, duration));
        }
    }
}