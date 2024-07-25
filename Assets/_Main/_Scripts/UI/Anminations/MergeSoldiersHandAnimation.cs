using DG.Tweening;
using UnityEngine;

namespace _Main._Scripts.UI.Anminations
{
    public class MergeSoldiersHandAnimation : MonoBehaviour
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform hand;

        private void OnEnable()
        {
            hand.localPosition = startPoint.localPosition;
            DOTween.Sequence().AppendInterval(0.5f).Append(hand.DOScale(Vector3.one * 0.7f, 0.3f))
                .Append(hand.DOLocalJump(endPoint.localPosition, 100f, 1, 2f).Append(hand.DOScale(Vector3.one, 0.3f))
                    .OnComplete(() => hand.localPosition = startPoint.localPosition)).SetLoops(-1);
        }
    }
}