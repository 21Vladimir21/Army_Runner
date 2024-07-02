using _Main._Scripts.CrowdLogic;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.UI;
using UnityEngine;
using CameraType = _Main._Scripts.Services.Cameras.CameraType;

namespace _Main._Scripts.Level.StateMachine.States
{
    public class PlayState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly GameView _gameView;
        private readonly CameraService _cameraService;
        private readonly Saves _saves;
        private readonly Player _player;
        private readonly Soldiers _soldiers;
        private readonly LevelService _levelService;

        public PlayState(IStateSwitcher stateSwitcher, GameView gameView, CameraService cameraService, Saves saves,
            Player player, Soldiers soldiers, LevelService levelService)
        {
            _stateSwitcher = stateSwitcher;
            _gameView = gameView;
            _cameraService = cameraService;
            _saves = saves;
            _player = player;
            _soldiers = soldiers;
            _levelService = levelService;
        }

        public void Enter()
        {
            _gameView.Open();
            _cameraService.SwitchToFromType(CameraType.Game);
            _player.OnStart.Invoke();
            _player.Crowd.ResetBoostsRatios(1,1,1);//TODO:Сделать загрузку данных прокачки толпы и тут тоже


            _player.gameObject.SetActive(true);
            foreach (var soldiersLevel in _saves.InGameSoldiers)
            {
                var soldier = Object.Instantiate(_soldiers.GetSoldierFromLevel(soldiersLevel.Level));
                _player.Crowd.AddToCrowdAndSetPosition(soldier);
            }

            _saves.InvokeSave();
            _levelService.CurrentLevel.Finish.OnFinished.AddListener(Finished);
        }

        public void Exit()
        {
            _gameView.Close();
            
        }

        public void Update()
        {
            if (_player.Crowd.SoldiersCount <= 0) _stateSwitcher.SwitchState<GameOverState>();
        }

        private void Finished()
        {
            _stateSwitcher.SwitchState<FinishState>();
        }
    }
}