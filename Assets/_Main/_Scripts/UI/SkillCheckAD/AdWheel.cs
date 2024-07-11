using System;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using DG.Tweening;
using LocalizationSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Main._Scripts.UI.SkillCheckAD
{
    public class AdWheel : MonoBehaviour
    {
        [SerializeField] private Transform arrowTransform;
        [SerializeField] private FormattableLocalizationTextTMP currentX;
        [SerializeField] private TMP_Text currentCountReward;
        [SerializeField] private Button claimButton;
        [SerializeField] private float cycleDuration;
        [SerializeField] private RewardMultiplierRanges[] adWheelValuesArray;
        private int _currentReward;
        private Saves _save;
        private bool _onLeft;
        public UnityEvent RewardCallback { get; } = new();


        private void Start()
        {
            _save = ServiceLocator.Instance.GetServiceByType<SavesService>().Saves;
            claimButton.onClick.AddListener(Claim);
            RotateTo();
        }

        public void SetCurrentReward(int currentReward)
        {
            _currentReward = currentReward;
        }

        private void Update()
        {
            var rotateValue = arrowTransform.rotation.eulerAngles.z;

            foreach (var values in adWheelValuesArray)
            {
                if (rotateValue >= values.minValue && rotateValue < values.maxValue)
                {
                    currentX.SetValue(values.xValue);
                    currentCountReward.text = (_currentReward * values.xValue).ToString();
                }
            }
        }


        private void Claim()
        {
            var rotateValue = arrowTransform.rotation.eulerAngles.z;

            foreach (var values in adWheelValuesArray)
            {
                if (rotateValue >= values.minValue && rotateValue < values.maxValue)
                {
                    //TODO: Сделать рекламу 
                    var money = _currentReward * values.xValue;
                    _save.AddMoney(money);
                    RewardCallback.Invoke();
                }
            }
        }

        private void RotateTo(float value = 180)
        {
            arrowTransform.DOLocalRotate(new Vector3(0, 0, value), cycleDuration / 2, RotateMode.WorldAxisAdd)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    RotateTo(_onLeft ? 180 : -180);
                    _onLeft = !_onLeft;
                });
        }
    }

    [Serializable]
    public struct RewardMultiplierRanges
    {
        [Range(0, 180)] public int minValue;
        [Range(0, 180)] public int maxValue;
        [Range(1, 3)] public int xValue;

        public RewardMultiplierRanges(int minValue, int maxValue, int xValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.xValue = xValue;
        }
    }
}