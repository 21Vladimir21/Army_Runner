using _Main._Scripts.PlayerLogic.StateMachine.States;

namespace _Main._Scripts.PlayerLogic.StateMachine
{
    public interface IStateSwitcher
    {
        void SwitchState<TState>() where TState : IState;
    }
}