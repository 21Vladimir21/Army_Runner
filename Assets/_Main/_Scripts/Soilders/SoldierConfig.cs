using _Main._Scripts.MergeLogic;
using UnityEngine;

namespace _Main._Scripts.Soilders
{
    [CreateAssetMenu(fileName = "SoldierConfig", menuName = "GameConfigs/SoldiersSettings/SoldierConfig", order = 0)]
    public class SoldierConfig : ScriptableObject
    {
        [field: SerializeField] public SoldiersLevels SoldiersLevel { get; private set; }

        [Range(0.1f, 10f)] public float fireRate;

        [Header("Bullet settings")]
        [Range(3f, 20f)] public float bulletLifeTime;
        [Range(0.1f, 10f)] public float bulletSpeed;
        [Range(1f, 1000)] public int bulletDamage;
    }
}