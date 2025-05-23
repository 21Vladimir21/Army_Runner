using System.Collections.Generic;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.MergeLogic.DragAndDropLogic;
using _Main._Scripts.Soilders;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Main._Scripts.CrowdLogic
{
    public class SoldiersPool
    {
        private readonly SoldiersPoolConfig _poolConfig;

        private readonly Transform _soldierParent;
        private readonly List<ISoldier> spawnedSoldiers = new();

        public SoldiersPool(SoldiersPoolConfig poolConfig, Transform soldierParent)
        {
            _poolConfig = poolConfig;
            _soldierParent = soldierParent;

            SpawnSoldiers<Soldier>(_poolConfig.SoldiersData);
            SpawnSoldiers<DraggableObject>(_poolConfig.MergeSoldiersData);
        }

        public void ReturnSoldier(ISoldier soldier)
        {
            var castSoldier = soldier as Component;
            if (castSoldier != null)
            {
                castSoldier.gameObject.SetActive(false);
                castSoldier.transform.position = _soldierParent.position;
            }
        }

        public T GetSoldierFromLevel<T>(SoldiersLevels level) where T : class, ISoldier
        {
            foreach (var soldier in spawnedSoldiers)
            {
                var castSoldier = soldier as Component;
                if (soldier.GetType() == typeof(T) && soldier.Level == level &&
                    castSoldier.gameObject.activeSelf == false)
                {
                    castSoldier.gameObject.SetActive(true);
                    return soldier as T;
                }
            }

            var spawnedSoldier = SpawnSoldier<T>(level);
            spawnedSoldiers.Add(spawnedSoldier);
            var castSpawnSoldier = spawnedSoldier as Component;
            castSpawnSoldier.gameObject.SetActive(true);
            return spawnedSoldier as T;
        }

        private void SpawnSoldiers<T>(List<SoldiersPoolConfig.SoldierPoolData> poolDatas) where T : class, ISoldier
        {
            foreach (var soldierData in poolDatas)
            {
                for (var i = 0; i < soldierData.SoldiersCount; i++)
                {
                    var spawnSoldier = SpawnSoldier<T>(soldierData.Level);
                    spawnedSoldiers.Add(spawnSoldier);
                }
            }
        }

        private ISoldier SpawnSoldier<T>(SoldiersLevels level) where T : class, ISoldier
        {
            var soldierPrefab = _poolConfig.Soldiers.GetSoldierFromLevel<T>(level);
            if (soldierPrefab == null)
            {
                Debug.LogError($"Soldier type {typeof(T)} prefab from level:{level} not found");
                return null;
            }

            var spawnSoldier = Object.Instantiate(soldierPrefab as Object, _soldierParent);
            spawnSoldier.GameObject().SetActive(false);
            return spawnSoldier as T;
        }
#if UNITY_EDITOR

        public void DebugSoldersLevelCount()
        {
            Dictionary<SoldiersLevels, int> soldiers = new();
            Dictionary<SoldiersLevels, int> mergeSoldiers = new();
            foreach (var soldier in spawnedSoldiers)
            {
                if (soldier.GetType() == typeof(DraggableObject))
                {
                    if (mergeSoldiers.ContainsKey(soldier.Level))
                        mergeSoldiers[soldier.Level]++;
                    else
                        mergeSoldiers[soldier.Level] = 1;
                }
                else if (soldier.GetType() == typeof(Soldier))
                {
                    if (soldiers.ContainsKey(soldier.Level))
                        soldiers[soldier.Level]++;
                    else
                        soldiers[soldier.Level] = 1;
                }
            }

            foreach (var soldier in soldiers)
                Debug.Log($"Солдат уровня{soldier.Key} : {soldier.Value}штук");
            foreach (var soldier in mergeSoldiers)
                Debug.Log($"Солдат для слияния уровня{soldier.Key} : {soldier.Value}штук");
        }
#endif
    }
}