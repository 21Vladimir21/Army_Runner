using UnityEditor;

namespace _Main._Scripts.Player.StateMachine.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        
        void Update();
    }
}