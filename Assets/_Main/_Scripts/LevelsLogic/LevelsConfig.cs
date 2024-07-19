using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.LevelsLogic
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "GameConfigs/LevelConfigs", order = 0)]
    public class LevelsConfig : ScriptableObject
    {
        [field: SerializeField] public List<LevelData> Levels { get; private set; }
        
        [ContextMenu("SetNames")]
        public void SetNames()
        {
            foreach (var level in Levels)
            {
                var indexOf = Levels.IndexOf(level) + 1;
                level.name = $"Level: {indexOf} Money: {level.Money}";
            }
        }
    }

    [Serializable]
    public class LevelData
    {
        [HideInInspector] public string name;
        [field: SerializeField] public Level Level { get; private set; }
        [field: SerializeField] public int Money { get; private set; }
    }
}