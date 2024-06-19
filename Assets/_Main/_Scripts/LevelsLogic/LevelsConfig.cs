using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.LevelsLogic
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "GameConfigs/LevelConfigs", order = 0)]
    public class LevelsConfig : ScriptableObject
    {
        [field: SerializeField] public List<LevelExample> Levels { get; private set; }
    }
}