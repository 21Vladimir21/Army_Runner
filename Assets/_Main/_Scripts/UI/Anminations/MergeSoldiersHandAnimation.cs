using DG.Tweening;
using UnityEngine;

namespace _Main._Scripts.UI.Anminations
{
    public class MergeSoldiersHandAnimation : MonoBehaviour
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform hand;
        [SerializeField] private float  jumpPower = 100f;

        private Sequence _sequence;
        

        private void OnEnable()
        {
            hand.localPosition = startPoint.localPosition;
            _sequence = DOTween.Sequence().AppendInterval(0.5f).Append(hand.DOScale(Vector3.one * 0.7f, 0.3f))
                .Append(hand.DOLocalJump(endPoint.localPosition, jumpPower, 1, 2f).Append(hand.DOScale(Vector3.one, 0.3f))
                    .OnComplete(() => hand.localPosition = startPoint.localPosition)).SetLoops(-1);
        }

        private void OnDisable()
        {
            hand.localScale = Vector3.one;
            _sequence.Kill();
        }
    }
}