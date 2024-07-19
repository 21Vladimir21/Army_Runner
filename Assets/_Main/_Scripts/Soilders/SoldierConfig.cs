using _Main._Scripts.MergeLogic;
using UnityEngine;

namespace _Main._Scripts.Soilders
{
    [CreateAssetMenu(fileName = "SoldierConfig", menuName = "GameConfigs/SoldiersSettings/SoldierConfig", order = 0)]
    public class SoldierConfig : ScriptableObject
    {
        [field: SerializeField] public SoldiersLevels SoldiersLevel { get; private set; }
        [field:Range(0, 360), SerializeField] public float YFinishRotation { get; private set; }
        

        [Header("Shooting settings")]
        [Range(0.1f, 10f)] public float fireRate; 
        [Range(0.1f, 10f)] public float bulletShotDelay; 
        [Range(1, 10)]public int oneShotBulletCount;

        [Header("Bullet settings")]
        [Range(1f, 20f)] public float bulletLifeTime;
        [Range(0.1f, 10f)] public float bulletSpeed;
        [Range(1f, 1000)] public float bulletDamage;
    }
}