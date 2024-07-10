using _Main._Scripts.CrowdLogic;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.Soilders;
using _Main._Scripts.UI;
using _Main._Scripts.UpgradeLogic;
using UnityEngine;
using CameraType = _Main._Scripts.Services.Cameras.CameraType;

namespace _Main._Scripts.Level.StateMachine.States
{
    public class PlayState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly GameView _gameView;
        private readonly SoldiersPool _soldiersPool;
        private readonly CameraService _cameraService;
        private readonly Saves _saves;
        private readonly Player _player;
        private readonly Soldiers _soldiers;
        private readonly UpgradeConfig _upgradeConfig;
        private readonly LevelService _levelService;

        public PlayState(IStateSwitcher stateSwitcher, GameView gameView, SoldiersPool soldiersPool,
            CameraService cameraService, Saves saves,
            Player player, Soldiers soldiers, UpgradeConfig upgradeConfig, LevelService levelService)
        {
            _stateSwitcher = stateSwitcher;
            _gameView = gameView;
            _soldiersPool = soldiersPool;
            _cameraService = cameraService;
            _saves = saves;
            _player = player;
            _soldiers = soldiers;
            _upgradeConfig = upgradeConfig;
            _levelService = levelService;
            _player.Crowd.OnTakeBoost.AddListener(_gameView.UpdateStats);
            _player.Crowd.OnSoldiersCountChanged.AddListener(_gameView.UpdateSoldiersCountText);
        }

        public void Enter()
        {
            _gameView.Open();
            _cameraService.SwitchToFromType(CameraType.Game);
            _player.OnStart.Invoke();

            UpdateUpgrades();

            _gameView.SetLevelText(_saves.CurrentLevel);
            _player.gameObject.SetActive(true);
            foreach (var soldiersLevel in _saves.InGameSoldiers)
            {
                var soldier = _soldiersPool.GetSoldierFromLevel<Soldier>(soldiersLevel.Level);
                _player.Crowd.AddToCrowdAndSetPosition(soldier);
            }

            _saves.InvokeSave();
            _levelService.CurrentLevel.Finish.OnFinished.AddListener(Finished);
            _gameView.UpdateSoldiersCountText(_player.Crowd.SoldiersCount);
        }

        private void UpdateUpgrades()
        {
            var bulletDamagePercentage = _saves.BulletDamagePercentage;
            var bulletSpeedPercentage = _saves.BulletSpeedPercentage;
            var fireRatePercentage = _saves.FireRatePercentage;
            _gameView.UpdateStats(bulletDamagePercentage, bulletSpeedPercentage, bulletSpeedPercentage);
            _player.Crowd.ResetBoostsPercentages(bulletDamagePercentage, bulletSpeedPercentage, fireRatePercentage);
        }

        public void Exit()
        {
            _gameView.Close();
        }

        public void Update()
        {

            var start =_levelService.CurrentLevel.PlayerSpawnPoint.position;
            var end =_levelService.CurrentLevel.Finish.transform.position;
            var levelLength = Vector3.Distance(start, end);
            var playerDistance = Vector3.Distance(_player.transform.position, start);
            var progress = playerDistance / levelLength;
            _gameView.UpdateProgressBar(progress);
            if (_player.Crowd.SoldiersCount <= 0) _stateSwitcher.SwitchState<GameOverState>();
        }

        private void Finished()
        {
            _stateSwitcher.SwitchState<FinishState>();
        }
    }
}