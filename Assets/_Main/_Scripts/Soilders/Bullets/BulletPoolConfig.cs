using UnityEngine;

namespace _Main._Scripts.Soilders.Bullets
{
    [CreateAssetMenu(fileName = "BulletPoolConfig", menuName = "GameConfigs/SoldiersSettings/BulletPoolConfig",
        order = 0)]
    public class BulletPoolConfig : ScriptableObject
    {
        [field: SerializeField, Range(1, 100)] public int StartBulletCount { get; private set; }
        [field: SerializeField] public Bullet BulletPrefab { get; private set; }
    }
}

