using System.Collections.Generic;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.Soilders;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main._Scripts.CrowdLogic
{
    [CreateAssetMenu(fileName = "Soldiers", menuName = "GameConfigs/SoldiersSettings/Soldiers", order = 0)]
    public class Soldiers : ScriptableObject
    {
        [SerializeField] public List<Soldier> soldiersPrefabs;
        [SerializeField] public List<DraggableObject> draggableObjectsPrefabs;

        public T GetSoldierFromLevel<T>(SoldiersLevels level) where T : class, ISoldier
        {
            var soldierType = typeof(T);
            if (soldierType == typeof(Soldier))
            {
                foreach (var soldier in soldiersPrefabs)
                    if (soldier.Config.SoldiersLevel == level)
                        return soldier as T;
            }
            else if (soldierType == typeof(DraggableObject))
            {
                foreach (var soldier in draggableObjectsPrefabs)
                    if (soldier.Level == level)
                        return soldier as T;
            }

            return null;
        }
    }
}