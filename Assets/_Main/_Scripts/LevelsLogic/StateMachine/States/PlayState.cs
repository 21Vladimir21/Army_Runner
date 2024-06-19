using _Main._Scripts.Player.StateMachine.States;
using UnityEngine;

namespace _Main._Scripts.Level.StateMachine.States
{
    public class PlayState : IState
    {
        public void Enter()
        {
            Debug.Log("EnterToPlayState");
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}