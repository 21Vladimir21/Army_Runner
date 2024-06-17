using System.Collections.Generic;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.Player.StateMachine;
using _Main._Scripts.Soilders;
using UnityEngine;

namespace _Main._Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerConfig Config { get; private set; }
        [SerializeField] private List<Transform> crowdPoints;

        [SerializeField] private BulletPool bulletPool;
        


        private PlayerStateMachine _stateMachine;
        public Crowd Crowd { get; private set; }
        private bool _mouseInput;

        public Transform Transform => transform;
        public bool MouseInput => _mouseInput;

        private void Start()
        {
            Crowd = new Crowd(crowdPoints,Config);
            _stateMachine = new PlayerStateMachine(this);
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
                soldier.InvitedToCrowd(bulletPool);
                Crowd.AddToCrowd(soldier);
            }
        }
    }
}