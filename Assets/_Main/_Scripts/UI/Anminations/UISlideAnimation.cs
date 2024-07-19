using DG.Tweening;
using UnityEngine;

namespace _Main._Scripts.UI.Anminations
{
    public class UISlideAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 startMovePosition;
        [SerializeField] private MoveAxis moveAxis;

        [SerializeField] private float delay;
        [SerializeField] private Ease ease;
        private Sequence _sequence;
        private Vector3 _defaultPosition;

        private void Awake() => _defaultPosition = transform.localPosition;

        private void OnEnable()
        {
            Vector3 targetPosition =  transform.localPosition;

            switch (moveAxis)
            {
                case MoveAxis.X:
                    targetPosition.x = startMovePosition.x;
                    break;
                case MoveAxis.Y:
                    targetPosition.y = startMovePosition.y;
                    break;
                case MoveAxis.Z:
                    targetPosition.z = startMovePosition.z;
                    break;
                default:
                    break;
            }
            transform.localPosition = targetPosition;
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOLocalMove(_defaultPosition, delay)).SetEase(ease);
        }
    }

    internal enum MoveAxis
    {
        X,
        Y,
        Z
    }
}