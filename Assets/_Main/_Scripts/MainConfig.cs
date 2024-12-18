using _Main._Scripts.CrowdLogic;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.MergeLogic.DragAndDropLogic;
using _Main._Scripts.Soilders.Bullets;
using _Main._Scripts.UpgradeLogic;
using UnityEngine;

namespace _Main._Scripts
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "GameConfigs/MainConfig", order = 0)]
    public class MainConfig : ScriptableObject
    {
        [field: SerializeField] public DragConfig DragConfig { get; private set; }
        [field: SerializeField] public LevelsConfig LevelsConfig { get; private set; }
        [field: SerializeField] public Soldiers Soldiers { get; private set; }
        [field: SerializeField] public UpgradeConfig UpgradeConfig { get; private set; }
        [field: SerializeField] public BulletPoolConfig BulletPoolConfig { get; private set; }
        [field: SerializeField] public SoldiersPoolConfig SoldiersPoolConfig { get; private set; }

    }
}