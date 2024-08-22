using _Main._Scripts.CrowdLogic;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.Soilders;
using _Main._Scripts.TutorialLogic;
using _Main._Scripts.UI;
using _Main._Scripts.UpgradeLogic;
using SoundService.Data;
using SoundService.Scripts;
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
        private readonly LevelService _levelService;
        private readonly TutorialService _tutorialService;
        private readonly AudioService _audioService;

        public PlayState(IStateSwitcher stateSwitcher, GameView gameView, SoldiersPool soldiersPool,
            CameraService cameraService, Saves saves,
            Player player, LevelService levelService)
        {
            _stateSwitcher = stateSwitcher;
            _gameView = gameView;
            _soldiersPool = soldiersPool;
            _cameraService = cameraService;
            _saves = saves;
            _player = player;
            _levelService = levelService;
            _player.Crowd.OnTakeBoost.AddListener(_gameView.UpdateStats);
            _player.Crowd.OnSoldiersCountChanged.AddListener(_gameView.UpdateSoldiersCountText);

            _gameView.RestartButton.onClick.AddListener(RestartGame);
            _tutorialService = ServiceLocator.Instance.GetServiceByType<TutorialService>();
            _audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();
        }

        public void Enter()
        {
            _audioService.PlaySound(Sound.RunMusic, true, 0.05f);
            _cameraService.ShowFade(() =>
            {
                _cameraService.SwitchToFromType(CameraType.Game);
                _cameraService.HideFade(() => _gameView.Open());
            });

            _player.OnStart.Invoke();

            UpdateUpgrades();

            _gameView.SetLevelText(_saves.CurrentLevelText);
            _player.gameObject.SetActive(true);
            foreach (var soldierFromSave in _saves.InGameSoldiers)
            {
                var soldier = _soldiersPool.GetSoldierFromLevel<Soldier>(soldierFromSave.Level);
                _player.Crowd.AddToCrowdAndSetPosition(soldier, soldierFromSave.Index);
            }

            _levelService.CurrentLevel.Finish.OnFinished.AddListener(Finished);
            _gameView.UpdateSoldiersCountText(_player.Crowd.SoldiersCount);
            _saves.InvokeSave();
        }

        private void UpdateUpgrades()
        {
            var bulletDamagePercentage = _saves.BulletDamagePercentage;
            var bulletSpeedPercentage = _saves.BulletSpeedPercentage;
            var fireRatePercentage = _saves.FireRatePercentage;
            _gameView.UpdateStats(bulletDamagePercentage, fireRatePercentage, bulletSpeedPercentage);
            _player.Crowd.ResetBoostsPercentages(bulletDamagePercentage, bulletSpeedPercentage, fireRatePercentage);
        }

        public void Exit()
        {
            _gameView.Close();
        }

        public void Update()
        {
            var start = _levelService.CurrentLevel.PlayerSpawnPoint.position;
            var end = _levelService.CurrentLevel.Finish.transform.position;
            var levelLength = Vector3.Distance(start, end);
            var playerDistance = Vector3.Distance(_player.transform.position, start);
            var progress = playerDistance / levelLength;
            _gameView.UpdateProgressBar(progress);
            if (_player.Crowd.SoldiersCount <= 0) _stateSwitcher.SwitchState<GameOverState>();
        }

        private void Finished()
        {
            _saves.LoseStreak = 0;
            _stateSwitcher.SwitchState<FinishState>();
        }

        private void RestartGame()
        {
            if (_saves.WasShowedTutorial == false) _tutorialService.ResetTutorial();
            _stateSwitcher.SwitchState<InitState>();
        }
    }
}