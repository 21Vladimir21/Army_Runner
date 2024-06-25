using _Main._Scripts.Level.StateMachine.States;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;

namespace _Main._Scripts.LevelsLogic.StateMachine.States
{
    public class InitState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private LevelSpawner _levelSpawner;
        private Saves _saves;
        private LevelsConfig _levelConfig;
        private bool _inited;

        public InitState(IStateSwitcher stateSwitcher,LevelSpawner levelSpawner, Saves saves, LevelsConfig levelConfig)
        {
            _stateSwitcher = stateSwitcher;
            _levelSpawner = levelSpawner;
            _saves = saves;
            _levelConfig = levelConfig;
            
        }

        public void Enter()
        {
            // var level = SetCurrentLevel();
            // _levelSpawner.SpawnLevel(level);
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

        private LevelExample SetCurrentLevel()
        {
            int level = _saves.CurrentLevel;
            return _levelConfig.Levels[level];
        }
    }
}