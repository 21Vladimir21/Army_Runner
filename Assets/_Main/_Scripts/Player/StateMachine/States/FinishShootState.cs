namespace _Main._Scripts.Player.StateMachine.States
{
    public class FinishShootState : IState
    {
        private readonly IStateSwitcher _switcher;
        private readonly Player _player;

        public FinishShootState(IStateSwitcher switcher, Player player)
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