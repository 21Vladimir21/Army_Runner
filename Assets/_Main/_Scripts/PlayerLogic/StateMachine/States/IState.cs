namespace _Main._Scripts.PlayerLogic.StateMachine.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        
        void Update();
    }
}