using System.Collections.Generic;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.Soilders;
using UnityEngine;

namespace _Main._Scripts.CrowdLogic
{
    public class SoldiersPool
    {
        private readonly SoldiersPoolConfig _poolConfig;
        private readonly Transform _soldierParent;
        private readonly List<ISoldier> spawnedSoldiers = new();
        private readonly List<ISoldier> spawnedMergeSoldiers = new();

        public SoldiersPool(SoldiersPoolConfig poolConfig, Transform soldierParent)
        {
            _poolConfig = poolConfig;
            _soldierParent = soldierParent;

            SpawnSoldiers<Soldier>(_poolConfig.SoldiersData, spawnedSoldiers);
            SpawnSoldiers<DraggableObject>(_poolConfig.MergeSoldiersData, spawnedMergeSoldiers);
        }


        private void SpawnSoldiers<T>(List<SoldiersPoolConfig.SoldierPoolData> poolDatas, List<ISoldier> addList)
            where T : class, ISoldier
        {
            foreach (var soldierData in poolDatas)
            {
                var soldierPrefab = _poolConfig.SoldierPrefabs.GetSoldierFromLevel<T>(soldierData.Level);
                if (soldierPrefab == null) continue;
                for (var i = 0; i < soldierData.SoldiersCount; i++)
                {
                    var spawnSoldier = Object.Instantiate(soldierPrefab as Object, _soldierParent);
                    addList.Add(spawnSoldier as T);
                }
            }
        }
        public T GetSoldierFromLevel<T>(SoldiersLevels level) where T : class, ISoldier
        {
            var soldierType = typeof(T);
            if (soldierType == typeof(Soldier))
            {
                
                foreach (var soldier in spawnedSoldiers)
                    if (soldier.Level == level)
                        return soldier as T;
            }
            else if (soldierType == typeof(DraggableObject))
            {
                foreach (var soldier in spawnedMergeSoldiers)
                    if (soldier.Level == level)
                        return soldier as T;
            }

            return null;
        }
    }
}