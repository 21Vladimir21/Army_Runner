using System.Collections.Generic;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Soilders;
using _Main._Scripts.Soilders.Bullets;
using UnityEngine;

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

        public Transform Transform => transform;
        public bool MouseInput => _mouseInput;


        private Vector3 _startPoint;

        public void Init(Saves saves)
        {
            _saves = saves;
            Crowd = new Crowd(crowdPoints, Config, bulletPoolConfig);
            _stateMachine = new PlayerStateMachine(this);
            _startPoint = transform.position;
        }

        public void GameOver()
        {
            transform.position = _startPoint;
            _mouseInput = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) _mouseInput = true;
            if (Input.GetMouseButtonUp(0)) _mouseInput = false;

            _stateMachine.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Soldier soldier))
            {
                if (soldier.InCrowd) return;
                Crowd.AddToCrowd(soldier);
                _saves.ReserveSoldiers.Add(soldier.Config.SoldiersLevel);
                _saves.InvokeSave(); //TODO: убрать нахуй отсюда эту хуету 
            }
        }
    }
}