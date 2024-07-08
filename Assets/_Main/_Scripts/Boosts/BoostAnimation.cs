using DG.Tweening;
using UnityEngine;

namespace _Main._Scripts.Boosts
{
    public class BoostAnimation : MonoBehaviour
    {
        [SerializeField] private float maxYposition = 0.84f;
        [SerializeField] private float minYposition = 0.35f;
        [SerializeField] private float rotateSpeed = 5f;
        [SerializeField] private float cycleSpeed = 2.5f;


        private void OnEnable()
        {
            MoveUp();
            transform.DOLocalRotate(new Vector3(0, 360, 0), rotateSpeed, RotateMode.FastBeyond360).SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        private void MoveUp() => transform.DOLocalMoveY(maxYposition, cycleSpeed / 2).SetEase(Ease.InOutSine)
            .OnComplete(MoveDown);

        private void MoveDown() => transform.DOLocalMoveY(minYposition, cycleSpeed / 2).SetEase(Ease.InOutSine)
            .OnComplete(MoveUp);
    }
}