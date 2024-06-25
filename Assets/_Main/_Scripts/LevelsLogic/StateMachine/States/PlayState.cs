using _Main._Scripts.CrowdLogic;
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

        public PlayState(IStateSwitcher stateSwitcher, GameView gameView, CameraService cameraService, Saves saves,
            Player player, Soldiers soldiers)
        {
            _stateSwitcher = stateSwitcher;
            _gameView = gameView;
            _cameraService = cameraService;
            _saves = saves;
            _player = player;
            _soldiers = soldiers;
        }

        public void Enter()
        {
            _gameView.Open();
            _cameraService.SwitchToFromType(CameraType.Game);

            _player.gameObject.SetActive(true);
            foreach (var soldiersLevel in _saves.InGameSoldiers)
            {
                var soldier = Object.Instantiate(_soldiers.GetSoldierFromLevel(soldiersLevel));
                _player.Crowd.AddToCrowdAndSetPosition(soldier);
            }
            _saves.InvokeSave();

        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}