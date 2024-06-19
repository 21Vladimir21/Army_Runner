using UnityEngine;

namespace _Main._Scripts.LevelsLogic
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        
        public void SpawnLevel(LevelExample level)
        {
            Instantiate(level, spawnPoint);
        }
    }
}