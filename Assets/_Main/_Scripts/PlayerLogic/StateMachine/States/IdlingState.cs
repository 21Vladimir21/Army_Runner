using _Main._Scripts.Soilders;

namespace _Main._Scripts.PlayerLogic.StateMachine.States
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
            _player.Crowd.SetAnimationForAllSoldiers(SoldierAnimationTriggers.Idling);
        }

        public void Exit()
        {
        }

        public void Update()
        {
            if (_player.MouseInput) 
                _switcher.SwitchState<MovementState>();
        }
    }
}