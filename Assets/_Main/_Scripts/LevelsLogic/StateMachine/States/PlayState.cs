using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
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
        private readonly Player _player;

        public PlayState(IStateSwitcher stateSwitcher, GameView gameView, CameraService cameraService,Player player)
        {
            _stateSwitcher = stateSwitcher;
            _gameView = gameView;
            _cameraService = cameraService;
            _player = player;
        }

        public void Enter()
        {
            _cameraService.SwitchToFromType(CameraType.Game);
            _player.gameObject.SetActive(true);
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}