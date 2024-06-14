using TMPro;
using UnityEngine;

namespace _Main._Scripts.Player.StateMachine.States
{
    public class IdlingState : IState
    {
        private IStateSwitcher _switcher;
        private Player _player;

        public IdlingState(IStateSwitcher switcher, Player player)
        {
            _switcher = switcher;
            _player = player;
        }

        public void Enter()
        {
            Debug.Log("IsIdlingState");
        }

        public void Exit()
        {
            Debug.Log("ExitIsIdlingState");
        }

        public void Update()
        {
            if (_player.MouseInput) 
                _switcher.SwitchState<MovementState>();
        }
    }
}