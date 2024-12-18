using System.Collections.Generic;
using System.Linq;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.PlayerLogic.StateMachine.States;

namespace _Main._Scripts.PlayerLogic.StateMachine
{
    public class PlayerStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;

        public PlayerStateMachine(Player player)
        {
            _states = new List<IState>()
            {
                new WaitingState(this,player),
                new IdlingState(this, player),
                new MovementState(this, player),
                new PlayerGameOverState(this,player),
            };

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<TState>() where TState : IState
        {
            var state = _states.FirstOrDefault(state => state is TState);

            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void Update() => _currentState.Update();
    }
}