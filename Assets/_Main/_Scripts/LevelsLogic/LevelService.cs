using _Main._Scripts.Services;
using UnityEngine;

namespace _Main._Scripts.LevelsLogic
{
    public class LevelService : IService
    {
        private Transform _spawnPoint;
        private LevelsConfig _levelsConfig;
        public Level CurrentLevel { get; private set; }

        public LevelService(Transform spawnPoint,LevelsConfig levelsConfig)
        {
            _spawnPoint = spawnPoint;
            _levelsConfig = levelsConfig;
        }
        public void SpawnLevel(int levelNumber)
        {
            if (CurrentLevel!=null) Object.Destroy(CurrentLevel.gameObject);
            CurrentLevel = Object.Instantiate(_levelsConfig.Levels[levelNumber].Level, _spawnPoint);
        }

        public int GetLevelMoneyReward( int levelNumber) => _levelsConfig.Levels[levelNumber].Money;
    }
}