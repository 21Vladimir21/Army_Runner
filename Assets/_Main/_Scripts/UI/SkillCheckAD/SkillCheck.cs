using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Main._Scripts.UI.SkillCheckAD
{
    public class SkillCheck : MonoBehaviour
    {
        [SerializeField] private Transform arrowTransform;
        [SerializeField] private TMP_Text currentX;
        [SerializeField] private TMP_Text currentCountReward;
        [SerializeField] private Button claimButton;
        [SerializeField] private float cycleDuration;
        [SerializeField] private AdWheelValues[] adWheelValuesArray;


        private void Start()
        {
            claimButton.onClick.AddListener(Claim);
            RotateToRight();
        }

        public void Init(int money)
        {
        }

     

        private void Claim()
        {

            var rotateValue = arrowTransform.rotation.eulerAngles.z;

            foreach (var values in adWheelValuesArray)
            {
                if (rotateValue >= values.minValue && rotateValue < values.maxValue)
                {
                    Debug.Log($"Current X - {values.xValue}X");
                }
            }
        }

        private void RotateToRight()
        {
            arrowTransform.DOLocalRotate(new Vector3(0, 0, 180), cycleDuration / 2, RotateMode.WorldAxisAdd)
                .SetEase(Ease.Linear)
                .OnComplete(RotateToLeft);
        }

        private void RotateToLeft()
        {
            arrowTransform.DOLocalRotate(new Vector3(0, 0, -180), cycleDuration / 2, RotateMode.WorldAxisAdd)
                .SetEase(Ease.Linear)
                .OnComplete(RotateToRight);
        }
    }

    [Serializable]
    public struct AdWheelValues
    {
        public int minValue;
        public int maxValue;
        [Range(1, 3)] public int xValue;

        public AdWheelValues(int minValue, int maxValue, int xValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.xValue = xValue;
        }
    }
}