using _Main._Scripts.Level.StateMachine.States;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;

namespace _Main._Scripts.LevelsLogic.StateMachine.States
{
    public class InitState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private LevelService _levelService;
        private Saves _saves;
        private LevelsConfig _levelConfig;
        private readonly Player _player;
        private bool _inited;

        public InitState(IStateSwitcher stateSwitcher, LevelService levelService, Saves saves, LevelsConfig levelConfig,
            Player player)
        {
            _stateSwitcher = stateSwitcher;
            _levelService = levelService;
            _saves = saves;
            _levelConfig = levelConfig;
            _player = player;
        }

        public void Enter()
        {
            var level = SetCurrentLevel();
            _levelService.SpawnLevel(level);
            _player.ResetPlayer(_levelService.CurrentLevel.PlayerSpawnPoint);
            _inited = true;
        }

        public void Exit()
        {
            _inited = false;
        }

        public void Update()
        {
            if (_inited) _stateSwitcher.SwitchState<MergeState>();
        }

        private Level SetCurrentLevel()
        {
            int level = _saves.CurrentLevel;
            return _levelConfig.Levels[level];
        }
    }
}