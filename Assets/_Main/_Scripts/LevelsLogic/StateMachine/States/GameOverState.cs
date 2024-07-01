using _Main._Scripts.Level.StateMachine.States;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.UI;

namespace _Main._Scripts.LevelsLogic.StateMachine.States
{
    public class GameOverState : IState

    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly GameOverView _gameOverView;
        private readonly Player _player;

        public GameOverState(IStateSwitcher stateSwitcher,GameOverView gameOverView , Player player)
        {
            _stateSwitcher = stateSwitcher;
            _gameOverView = gameOverView;
            _player = player;
            _gameOverView.StartGameButton.onClick.AddListener(RestartGame);
        }

        public void Enter()
        {
            _gameOverView.Open();
            _player.gameObject.SetActive(false);
        }

        public void Exit()
        {
            _player.GameOver();
            _gameOverView.Close();
            
        }

        public void Update()
        {
        }

        private void RestartGame()
        {
            _stateSwitcher.SwitchState<MergeState>();
            _player.Restart();
        }
    }
}