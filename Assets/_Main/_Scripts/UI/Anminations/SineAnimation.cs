using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI.Anminations
{
    [RequireComponent(typeof(Mask)), RequireComponent(typeof(RectMask2D))]
    public class SineAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _sineEffect;
        [SerializeField] private float _xLocalStartPosition = -500;
        [SerializeField] private float _xLocalEndPosition = 500;

        [Min(0), SerializeField] private float _moveDuration = 1.5f;
        [Min(0), SerializeField] private float _moveDelay = 1;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        private Tween _animationTween;

        private void OnValidate()
        {
            if (transform.childCount > 1 && transform.GetChild(0).name.Contains("sine",StringComparison.OrdinalIgnoreCase))
            {
                _sineEffect ??= transform.GetChild(0);
            }

            if (_xLocalStartPosition > _xLocalEndPosition)
            {
                _xLocalEndPosition = _xLocalStartPosition + 1;
            }
        }

        private void OnEnable() => PlayAnimation();
        private void OnDisable() => StopAnimation();

        private void PlayAnimation()
        {
            _animationTween?.Kill();
            _sineEffect.transform.localPosition = Vector3.right * _xLocalStartPosition;

            _animationTween = _sineEffect
                .DOLocalMoveX(_xLocalEndPosition, _moveDuration)
                .SetEase(_ease)
                .SetLoops(-1)
                .SetDelay(_moveDelay);
        }

        private void StopAnimation()
        {
            _animationTween?.Kill();
        }
    }
}