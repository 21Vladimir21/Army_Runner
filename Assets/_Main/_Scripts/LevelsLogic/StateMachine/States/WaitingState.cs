using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;

namespace _Main._Scripts.LevelsLogic.StateMachine.States
{
    public class WaitingState :IState
    {
        private readonly IStateSwitcher _switcher;
        private readonly Player _player;

        public WaitingState(IStateSwitcher switcher, Player player)
        {
            _switcher = switcher;
            _player = player;
            _player.OnStart.AddListener(StartGame);
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

        private void StartGame()
        {
            _switcher.SwitchState<IdlingState>();
        }
    }
}