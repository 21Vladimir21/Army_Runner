using System.Collections.Generic;
using _Main._Scripts.Boosts;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.Obstacles;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Soilders;
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
        public UnityEvent OnRestart { get; } = new();
        public UnityEvent OnStart { get; private set; } = new();
        public Transform Transform => transform;
        public bool MouseInput => _mouseInput;


        private Vector3 _startPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Soldier soldier))
            {
                if (soldier.InCrowd) return;
                Crowd.AddToCrowd(soldier);
            }

            if (other.TryGetComponent(out Boost boost))
            {
                Crowd.UpdateBulletBoostPercentages(boost);
                boost.Take();
            }
            if (other.TryGetComponent(out PickUpMoney pickUpMoney))
            {
                _saves.AddMoney(pickUpMoney.Count);
                pickUpMoney.TakeMoney();
            }
        }
        

        public void Init(Saves saves,BulletPool bulletPool,SoldiersPool soldiersPool)
        {
            _saves = saves;
            Crowd = new Crowd(crowdPoints,Config, bulletPool, soldiersPool,saves);
            _stateMachine = new PlayerStateMachine(this);
            _startPoint = transform.position;
        }

        public void ResetPlayer(Transform playerStartPoint)
        {
            transform.position = playerStartPoint.position;
            Crowd.ResetCrowd();
        }

        public void GameOver()
        {
            transform.position = _startPoint;
            _mouseInput = false;
            _stateMachine.SwitchState<PlayerGameOverState>();
        }

        public void Restart() => OnRestart.Invoke();

        public void FinishedShooting() // TODO:Однозначно надо сделать это отдельным состоянием 
        {
            _stateMachine.SwitchState<WaitingState>();
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) _mouseInput = true;
            if (Input.GetMouseButtonUp(0)) _mouseInput = false;

            _stateMachine.Update();
        }
    }
}