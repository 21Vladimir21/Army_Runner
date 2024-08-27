using _Main._Scripts.LevelsLogic;
using _Main._Scripts.LevelsLogic.FinishLogic;
using _Main._Scripts.LevelsLogic.FinishLogic.Enemies;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.Soilders;
using _Main._Scripts.UI;
using Kimicu.YandexGames;
using SoundService.Data;
using SoundService.Scripts;
using UnityEngine;
using CameraType = _Main._Scripts.Services.Cameras.CameraType;

namespace _Main._Scripts.Level.StateMachine.States
{
    public class FinishState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly FinishView _finishView;
        private readonly CameraService _cameraService;
        private readonly Saves _saves;
        private readonly LevelService _levelService;
        private readonly Player _player;

        private int _enemyCount;
        private int _diedEnemiesCount;

        private int _collectedMoneyCount;
        private int _collectedSoldiersCount;

        private bool _canShoot;
        private Finish _finish;
        private readonly AudioService _audioService;

        public FinishState(IStateSwitcher stateSwitcher, FinishView finishView, CameraService cameraService,
            Saves saves, LevelService levelService, Player player)
        {
            _stateSwitcher = stateSwitcher;
            _finishView = finishView;
            _cameraService = cameraService;
            _saves = saves;
            _levelService = levelService;
            _player = player;

            _player.Crowd.OnTakeMoney.AddListener(value => _collectedMoneyCount += value);
            _player.Crowd.OnTakeSoldier.AddListener(() => _collectedSoldiersCount++);

            _finishView.NoThanksButton.onClick.AddListener(ToMerge);
            _finishView.ADWheel.RewardCallback.AddListener(ToMergeReward);
            
            _audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();

        }

        public void Enter()
        {
            _cameraService.SwitchToFromType(CameraType.FinishCamera, _levelService.CurrentLevel.FinishCameraPoint);
            _finishView.Open();
            _finishView.ADWheel.SetCurrentReward(_levelService.GetLevelMoneyReward(_saves.CurrentLevel));
            _finish = _levelService.CurrentLevel.Finish;
            _finish.FinishDeathZone.OnEnemyTouchZone.AddListener(GameOver);
            _enemyCount = _finish.Enemies.Count;
            _finishView.UpdateEnemyCountText(_enemyCount);

            foreach (var enemy in _finish.Enemies)
            {
                enemy.OnDie.AddListener(TryEndLevel);
                enemy.OnHit.AddListener(() => _player.Crowd.SetAnimationForAllSoldiers(SoldierAnimationTriggers.Dying));
            }

            _finish.SetSoldiersNewPosition(_player.Crowd.Soldiers, () =>
            {
                _canShoot = true;
                _finish.StartEnemiesAttach();
                _player.Crowd.SetAnimationForAllSoldiers(SoldierAnimationTriggers.FinishShooting);
                _player.Crowd.SetFinishShootingSettings();
            });
            _player.FinishedShooting();
        }

        public void Exit()
        {
            _collectedMoneyCount = 0;
            _collectedSoldiersCount = 0;
            _diedEnemiesCount = 0;
            _finish.StopEnemiesAttach();
            _finishView.Close();
        }

        public void Update()
        {
            if (_canShoot)
            {
                _player.Crowd.UpdateShootingCooldownForAllSoldiers();
                SetTargetsForSoldiers();
            }
        }

        private void SetTargetsForSoldiers()
        {
            foreach (var soldier in _player.Crowd.Soldiers)
            {
                var minDistance = float.MaxValue;
                Vector3 enemyPosition = Vector3.zero;
                foreach (var enemy in _finish.Enemies)
                {
                    var distance = Vector3.Distance(soldier.transform.position, enemy.transform.position);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        enemyPosition = enemy.transform.position;
                    }
                }

                soldier.SetLookDirection(enemyPosition);
            }
        }

        private void TryEndLevel(Enemy enemy)
        {
            _diedEnemiesCount++;
            enemy.OnDie.RemoveListener(TryEndLevel);
            _finish.Enemies.Remove(enemy);
            _finishView.UpdateEnemyCountText(_enemyCount - _diedEnemiesCount);
            if (_diedEnemiesCount >= _enemyCount)
            {
                _finishView.NoThanksButton.gameObject.SetActive(true);
                _canShoot = false;
                _player.Crowd.SetAnimationForAllSoldiers(SoldierAnimationTriggers.Dance);
                _player.Crowd.SaveCurrentSoldiers();
                _finishView.ShowWinPanel(_collectedMoneyCount, _collectedSoldiersCount);
                _audioService.PlaySound(Sound.WinMusic,volumeScale:1.7f);
                _saves.AddMoney(_levelService.GetLevelMoneyReward(_saves.CurrentLevel));
                _saves.SetNextLevel();
            }
        }

        private void ToMerge()
        {
            if (_saves.CanShowAd && _saves.AdEnabled && Advertisement.AdvertisementIsAvailable)
            {
                Advertisement.ShowInterstitialAd(Audio.MuteAllAudio, () =>
                {
                    Audio.UnMuteAllAudio();
                    _stateSwitcher.SwitchState<InitState>();
                });
            }
            else
                _stateSwitcher.SwitchState<InitState>();
        }

        private void ToMergeReward()
        {
            _stateSwitcher.SwitchState<InitState>();
            YandexMetrika.Event("X3Reward");
        }

        private void GameOver() => _stateSwitcher.SwitchState<GameOverState>();
    }
}