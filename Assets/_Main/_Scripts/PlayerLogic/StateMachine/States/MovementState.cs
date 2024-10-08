using _Main._Scripts.CrowdLogic;
using _Main._Scripts.Soilders;
using Kimicu.YandexGames;
using UnityEngine;
using Device = Agava.WebUtility.Device;

namespace _Main._Scripts.PlayerLogic.StateMachine.States
{
    public class MovementState : IState
    {
        private readonly float _speedRatio;
        private readonly float _maxLeftPosition;
        private readonly float _maxRightPosition;
        private readonly float _maxXDelta;
        private readonly float _sensitivity;
        private readonly float _keySensitivity;
        private readonly float _xDamping;

        private readonly Player _player;
        private readonly Crowd _crowd;
        private readonly IStateSwitcher _switcher;

        private Vector3 _startDragPosition;

        public MovementState(IStateSwitcher switcher, Player player)
        {

            _switcher = switcher;
            _speedRatio = player.Config.speed;
            _maxLeftPosition = -player.Config.maxLeftRightPosition;
            _maxRightPosition = player.Config.maxLeftRightPosition;
            _maxXDelta = player.Config.maxXDragDelta;
            _sensitivity = Device.IsMobile ? player.Config.xSensitivityMobile : player.Config.xSensitivity;
            _keySensitivity = player.Config.xKeySensitivity;
            _xDamping = player.Config.xDampingRatio;
            _player = player;
            _crowd = player.Crowd;
        }

        public void Enter()
        {
            _startDragPosition = Input.mousePosition;
            _player.Crowd.SetAnimationForAllSoldiers(SoldierAnimationTriggers.IsRunning);
        }

        public void Exit()
        {
        }

        public void Update()
        {
            if (_player.MouseInput == false)
                _switcher.SwitchState<IdlingState>();

            ClampedMove(_player.Transform, Time.deltaTime);
            _crowd.UpdateSoldiers();
        }

        private void ClampedMove(Transform playerTransform, float deltaTime)
        {
            var position = playerTransform.position;
            position += GetMoveDirection(deltaTime);

            var clamp = Mathf.Clamp(position.x, _maxLeftPosition, _maxRightPosition);
            position = new Vector3(clamp, position.y, position.z);

            playerTransform.position = position;
            SetXDirection(playerTransform, deltaTime);
            SetKeyboardDirection(playerTransform, deltaTime);
        }

        private Vector3 GetMoveDirection(float deltaTime)
        {
            var forwardDirection = Vector3.forward;

            var moveDirection = forwardDirection.normalized * _speedRatio;
            return moveDirection * deltaTime;
        }

        private void SetXDirection(Transform xPosition, float deltaTime)
        {
            var dragDelta = Input.mousePosition.x - _startDragPosition.x;
            var direction = Mathf.Clamp(dragDelta / Screen.width, -_maxXDelta, _maxXDelta);

            var position = xPosition.position;
            position = new Vector3(position.x + direction, position.y, position.z);
            xPosition.position = Vector3.Lerp(xPosition.position, position, deltaTime * _sensitivity);
            _startDragPosition = Vector3.Lerp(_startDragPosition, Input.mousePosition, deltaTime * _xDamping);
        }

        private void SetKeyboardDirection(Transform xPosition, float deltaTime)
        {
            var direction = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) direction = -_keySensitivity;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) direction = _keySensitivity;

            var position = xPosition.position;
            position = new Vector3(position.x + direction * deltaTime, position.y, position.z);
            xPosition.position = Vector3.Lerp(xPosition.position, position, deltaTime * _sensitivity);
        }
    }
}