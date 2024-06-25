namespace _Main._Scripts.PlayerLogic.StateMachine.States
{
    public class FinishShootState : IState
    {
        private readonly IStateSwitcher _switcher;
        private readonly PlayerLogic.Player _player;

        public FinishShootState(IStateSwitcher switcher, PlayerLogic.Player player)
        {
            _switcher = switcher;
            _player = player;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        private void Shoot()
        {
            
        }
        
        private void ChooseTarget()
        {
            
        }
    }
}