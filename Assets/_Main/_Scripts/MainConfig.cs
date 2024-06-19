using _Main._Scripts.LevelsLogic;
using _Main._Scripts.MergeLogic.DragAndDropLogic;
using UnityEngine;

namespace _Main._Scripts
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "GameConfigs/MainConfig", order = 0)]
    public class MainConfig : ScriptableObject
    {
        [field: SerializeField] public DragConfig DragConfig { get; private set; }
        [field: SerializeField] public LevelsConfig LevelsConfig{ get; private set; }
    }
}