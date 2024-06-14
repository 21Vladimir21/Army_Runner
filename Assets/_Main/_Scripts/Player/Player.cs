using _Main._Scripts.CrowdLogic;
using _Main._Scripts.Player.StateMachine;
using _Main._Scripts.Soilders;
using UnityEngine;

namespace _Main._Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerConfig Config { get; private set; }

        private PlayerStateMachine _stateMachine;
        private Crowd _crowd;
        private bool _mouseInput;

        public Transform Transform => transform;
        public bool MouseInput => _mouseInput;

        private void Start()
        {
            _stateMachine = new PlayerStateMachine(this);
            _crowd = new Crowd(Transform);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) _mouseInput = true;
            if (Input.GetMouseButtonUp(0)) _mouseInput = false;

            _stateMachine.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Soilder solder))
            {
                _crowd.AddToCrowd(solder);
            }
        }
    }
}