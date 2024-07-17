using System;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using DG.Tweening;
using Kimicu.YandexGames;
using LocalizationSystem.Components;
using SoundService.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Main._Scripts.UI.SkillCheckAD
{
    public class AdWheel : MonoBehaviour
    {
        [SerializeField] private Transform arrowTransform;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private FormattableLocalizationTextTMP currentX;
        [SerializeField] private FormattableLocalizationTextTMP levelReward;
        [SerializeField] private TMP_Text currentCountReward;
        [SerializeField] private Button claimButton;
        [SerializeField] private float cycleDuration;
        [SerializeField] private RewardMultiplierRanges[] adWheelValuesArray;

        private int _currentReward;
        private Saves _save;
        private Sequence _sequence;
        public UnityEvent RewardCallback { get; } = new();

        private void Start() => claimButton.onClick.AddListener(Claim);

        private void OnEnable()
        {
            arrowTransform.rotation = Quaternion.Euler(Vector3.zero);
            claimButton.interactable = true;

            _sequence = DOTween.Sequence();
            _save = ServiceLocator.Instance.GetServiceByType<SavesService>().Saves;
            StartRotate();
        }

        private void OnDisable() => _sequence.Kill();
        public void SetCurrentReward(int currentReward)
        {
            levelReward.SetValue(currentReward);
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
            //TODO: Логика получения отличается от того, что я делал раньше. Наверное надо будет сделать по другому
            var rotateValue = arrowTransform.rotation.eulerAngles.z;
            claimButton.interactable = false;
            _sequence.Kill();
            Advertisement.ShowVideoAd(Audio.MuteAllAudio, () =>
            {
                foreach (var values in adWheelValuesArray)
                {
                    if (rotateValue >= values.minValue && rotateValue < values.maxValue)
                    {
                        var money = _currentReward * values.xValue;
                        _save.AddMoney(money);
                        particle.Play();
                        RewardCallback.Invoke();
                    }
                }
            }, () => Audio.UnMuteAllAudio());
        }

        private void StartRotate()
        {
            _sequence.Append(arrowTransform
                .DOLocalRotate(new Vector3(0, 0, 180), cycleDuration / 2, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo));
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