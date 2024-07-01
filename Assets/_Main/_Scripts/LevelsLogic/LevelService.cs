using _Main._Scripts.Services;
using UnityEngine;

namespace _Main._Scripts.LevelsLogic
{
    public class LevelService : MonoBehaviour ,IService
    {
        [SerializeField] private Transform spawnPoint;
        public Level CurrentLevel { get; private set; }

        public void SpawnLevel(Level level)
        {
            if (CurrentLevel!=null) Destroy(CurrentLevel.gameObject);
            CurrentLevel = Instantiate(level, spawnPoint);
        }
    }
}