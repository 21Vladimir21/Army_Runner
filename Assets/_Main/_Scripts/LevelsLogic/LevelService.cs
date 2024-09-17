using System;
using _Main._Scripts.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Main._Scripts.LevelsLogic
{
    public class LevelService : IService
    {
        private Transform _spawnPoint;
        private LevelsConfig _levelsConfig;
        public Level CurrentLevel { get; private set; }
        private GameObject _currentLevelGameObject;
        private AsyncOperationHandle<GameObject> _handle;
        private AssetReference _reference;

        public LevelService(Transform spawnPoint, LevelsConfig levelsConfig)
        {
            _spawnPoint = spawnPoint;
            _levelsConfig = levelsConfig;
        }

        public void SpawnLevel(int levelNumber, Action callBack)
        {
            if (_currentLevelGameObject != null)
            {
                CurrentLevel.gameObject.SetActive(false);
                Addressables.ReleaseInstance(_currentLevelGameObject);
                Debug.Log($"Level deactivate and release");
            }

            var handle = _levelsConfig.Levels[levelNumber].Level.InstantiateAsync(_spawnPoint);
            handle.Completed += operationHandle =>
            {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _currentLevelGameObject = operationHandle.Result;
                    CurrentLevel = _currentLevelGameObject.GetComponent<Level>();
                    callBack.Invoke();
                    Debug.Log($"Level load and spawned");
                }
            };
        }

        public int GetLevelMoneyReward(int levelNumber) => _levelsConfig.Levels[levelNumber].Money;
    }
}