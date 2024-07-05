using System;
using System.Collections.Generic;
using _Main._Scripts.MergeLogic;
using UnityEngine;

namespace _Main._Scripts.CrowdLogic
{
    [CreateAssetMenu(fileName = "SoldiersPoolConfig", menuName = "GameConfigs/SoldierPool", order = 0)]
    public class SoldiersPoolConfig : ScriptableObject
    {
        [field: SerializeField] public Soldiers SoldierPrefabs { get; private set; }
        [field:SerializeField] public List<SoldierPoolData> SoldiersData{ get; private set; }
        [field:SerializeField] public List<SoldierPoolData> MergeSoldiersData{ get; private set; }

        [ContextMenu("UpdateLevels")]
        private void UpdateLevels()
        {
            UpdateSoldiersDataWithMissingLevels(SoldiersData);
            UpdateSoldiersDataWithMissingLevels(MergeSoldiersData);
        }

        private void UpdateSoldiersDataWithMissingLevels(List<SoldierPoolData> poolDatas)
        {
            foreach (var level in Enum.GetValues(typeof(SoldiersLevels)))
            {
                bool levelFound = false;

                foreach (var data in poolDatas)
                {
                    if (data.Level == (SoldiersLevels)level)
                    {
                        levelFound = true;
                        break;
                    }
                }

                if (!levelFound)
                {
                    SoldierPoolData newData = new SoldierPoolData();
                    newData.Level = (SoldiersLevels)level;
                    newData.name = level.ToString();
                    poolDatas.Add(newData);
                }
            }
        }

        [Serializable]
        public class SoldierPoolData
        {
            [HideInInspector] public string name;
            [field: SerializeField] public SoldiersLevels Level { get; set; }
            [field: Range(1, 50), SerializeField] public int SoldiersCount { get; private set; } = 10;
        }
    }
}