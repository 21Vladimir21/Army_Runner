using _Main._Scripts.Player.StateMachine.States;

namespace _Main._Scripts.Player.StateMachine
{
    public interface IStateSwitcher
    {
        void SwitchState<TState>() where TState : IState;
    }
}