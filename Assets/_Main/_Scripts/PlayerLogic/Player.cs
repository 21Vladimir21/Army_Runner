using System.Collections.Generic;
using _Main._Scripts.Boosts;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
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
        [SerializeField] private BulletPoolConfig bulletPoolConfig;

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
                var index = Crowd.AddToCrowd(soldier);
                _saves.ReserveSoldiers.Add(new Saves.Soldier(soldier.Config.SoldiersLevel, index));
                _saves.InvokeSave(); //TODO: убрать нахуй отсюда эту хуету 
            }

            if (other.TryGetComponent(out Boost boost))
                Crowd.UpdateBulletBoostRatio(boost);
        }

        public void Init(Saves saves)
        {
            _saves = saves;
            Crowd = new Crowd(crowdPoints, Config, bulletPoolConfig, 1,
                1, 1); //TODO:Сделать загрузку данных прокачки толпы
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

        public void FinishedShooting()
        {
            _stateMachine.SwitchState<WaitingState>();
            foreach (var soldier in Crowd.Soldiers) 
                soldier.IsFinishShooting = true;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) _mouseInput = true;
            if (Input.GetMouseButtonUp(0)) _mouseInput = false;

            _stateMachine.Update();
        }
    }
}