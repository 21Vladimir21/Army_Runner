using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.UpgradeLogic
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "GameConfigs/UpgradeConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        [field: SerializeField] public List<UpgradeData> DamageUpgradeRatios { get; private set; }
        [field: SerializeField] public List<UpgradeData> FireRateUpgradeRatios { get; private set; }
        [field: SerializeField] public List<UpgradeData> SpeedUpgradeRatios { get; private set; }

#if UNITY_EDITOR

        private void OnValidate()
        {
            SetNames(DamageUpgradeRatios);
            SetNames(FireRateUpgradeRatios);
            SetNames(SpeedUpgradeRatios);
        }

        private void SetNames(List<UpgradeData> list)
        {
            foreach (var data in list) data.name = $"Percentage: {data.Percentage} ___ Cost {data.Cost} ";
        }
#endif
    }

    [Serializable]
    public class UpgradeData
    {
        [HideInInspector] public string name;

        [field: Range(-100, 100), SerializeField]
        public float Percentage { get; private set; }

        [field: SerializeField] public int Cost { get; private set; }
    }
}