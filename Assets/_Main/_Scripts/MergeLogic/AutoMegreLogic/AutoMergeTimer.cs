using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.MergeLogic
{
    public class AutoMergeTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Button rewardButton;

        [SerializeField] private GameObject adText;
        [SerializeField] private Image fill;

        private const float StartTime = 300;
        private TimeSpan _lastTime;
        private float _currentTime;
        private bool _isFinished;

        public bool AutoMergeEnabled => !_isFinished;

        private void OnEnable()
        {
            CalculateTimeLeft();
        }

        private void OnDisable()
        {
            _lastTime = DateTime.UtcNow.TimeOfDay;
        }

        public void Update()
        {
            if (_isFinished) return;
            _currentTime -= Time.deltaTime;
            SetFillAndText();
            if (_currentTime <= 0)
            {
                Debug.Log($"Таймер закончил работу");
                _isFinished = true;
                adText.SetActive(true);
                timerText.gameObject.SetActive(false);
                rewardButton.interactable = true;
            }
        }

        public void ClaimReward()
        {
            adText.SetActive(false);
            _currentTime = StartTime;
            timerText.gameObject.SetActive(true);
            _isFinished = false;
            rewardButton.interactable = false;
                Debug.Log($"Таймер начал работу");
        }

        private void CalculateTimeLeft()
        {
            var currentDateTime = DateTime.UtcNow.TimeOfDay;
            _currentTime -= (float)currentDateTime.Subtract(_lastTime).TotalSeconds;
            if (_currentTime < 0) _currentTime = 0;
            Debug.Log($"Текущее время :{currentDateTime} , сохраненное время: {_lastTime} осталось времени:{_currentTime}");
        }

        private void SetFillAndText()
        {
            fill.fillAmount = _currentTime / StartTime;
            timerText.text = FormatFloatToTime(_currentTime);
        }

        private string FormatFloatToTime(float value)
        {
            int minutes = (int)Math.Floor(value / 60);
            int seconds = (int)value % 60;
            return minutes + ":" + seconds.ToString("D2");
        }
    }
}