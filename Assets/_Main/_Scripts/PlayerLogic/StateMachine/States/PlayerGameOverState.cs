using _Main._Scripts.LevelsLogic.StateMachine.States;

namespace _Main._Scripts.PlayerLogic.StateMachine.States
{
    public class PlayerGameOverState : IState
    {
        private readonly IStateSwitcher _switcher;
        private readonly Player _player;

        public PlayerGameOverState(IStateSwitcher switcher,Player player)
        {
            _switcher = switcher;
            _player = player;
            _player.OnRestart.AddListener(Restart);
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        private void Restart()
        {
            _switcher.SwitchState<WaitingState>();
        }
    }
}