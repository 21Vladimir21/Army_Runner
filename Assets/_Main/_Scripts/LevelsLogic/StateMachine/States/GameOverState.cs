using _Main._Scripts.LevelsLogic.FinishLogic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using _Main._Scripts.TutorialLogic;
using _Main._Scripts.UI;
using Kimicu.YandexGames;
using SoundService.Data;
using SoundService.Scripts;

namespace _Main._Scripts.LevelsLogic.StateMachine.States
{
    public class GameOverState : IState

    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly Saves _saves;
        private readonly GameOverView _gameOverView;
        private readonly Player _player;
        private readonly TutorialService _tutorialService;
        private readonly AudioService _audioService;

        public GameOverState(IStateSwitcher stateSwitcher, Saves saves, GameOverView gameOverView, Player player)
        {
            _stateSwitcher = stateSwitcher;
            _saves = saves;
            _gameOverView = gameOverView;
            _player = player;
            _gameOverView.BackButton.onClick.AddListener(RestartGame);
            _tutorialService = ServiceLocator.Instance.GetServiceByType<TutorialService>();
            _audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();
        }

        public void Enter()
        {
            _saves.LoseStreak++;
            _gameOverView.Open(() => _audioService.PlaySound(Sound.LoseMusic, volumeScale: 0.5f));
            _player.gameObject.SetActive(false);
            if (_saves.WasShowedTutorial == false) _tutorialService.ResetTutorial();
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
            if (_saves.CanShowAd && _saves.AdEnabled && Advertisement.AdvertisementIsAvailable)
            {
                Advertisement.ShowInterstitialAd(Audio.MuteAllAudio, () =>
                {
                    Audio.UnMuteAllAudio();
                    _stateSwitcher.SwitchState<InitState>();
                    _player.Restart();
                });
            }
            else
            {
                _stateSwitcher.SwitchState<InitState>();
                _player.Restart();
            }
        }
    }
}