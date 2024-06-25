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

        public Soldier GetSoldierFromLevel(SoldiersLevels level)
        {
            foreach (var soldier in soldiersPrefabs)
                if (soldier.Config.SoldiersLevel == level)
                    return soldier;

            return null;
        }
    }
}