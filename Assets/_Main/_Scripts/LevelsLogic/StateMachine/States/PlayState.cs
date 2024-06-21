using _Main._Scripts.Player.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using UnityEngine;

namespace _Main._Scripts.Level.StateMachine.States
{
    public class PlayState : IState
    {
        public PlayState()
        {
        }

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