using System;
using System.Collections.Generic;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Soilders.Bullets;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerConfig Config { get; private set; }

        [SerializeField] private List<Transform> crowdPoints;

        private PlayerStateMachine _stateMachine;
        public Crowd Crowd { get; private set; }
        private bool _mouseInput;
        private Saves _saves;
        public UnityEvent OnStart { get; private set; } = new();
        public Transform Transform => transform;

        public bool MouseInput
        {
            get => _mouseInput;
            set => _mouseInput = value;
        }


        private Vector3 _startPoint;

        public void Init(Saves saves, BulletPool bulletPool, SoldiersPool soldiersPool)
        {
            _saves = saves;
            Crowd = new Crowd(crowdPoints, Config, bulletPool, soldiersPool, saves);
            _stateMachine = new PlayerStateMachine(this);
            _startPoint = transform.position;
        }

        public void ResetPlayer(Transform playerStartPoint)
        {
            transform.position = playerStartPoint.position;
            Crowd.ResetCrowd();
            _stateMachine.SwitchState<WaitingState>();
        }

        public void GameOver()
        {
            transform.position = _startPoint;
            _mouseInput = false;
            _stateMachine.SwitchState<WaitingState>();
        }

        public void Restart()
        {
            _stateMachine.SwitchState<WaitingState>();
        }

        public void FinishedShooting() // TODO:Однозначно надо сделать это отдельным состоянием 
        {
            _stateMachine.SwitchState<WaitingState>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.UpArrow)) _mouseInput = true;
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.W) ||
                Input.GetKeyUp(KeyCode.LeftArrow) ||
                Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) ||
                Input.GetKeyUp(KeyCode.UpArrow)) _mouseInput = false;

            if (_mouseInput && Input.GetMouseButton(0) == false)
            {
                _mouseInput = false;
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.Update();
        }
    }
}