using System.Collections.Generic;
using System.Linq;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.Level.StateMachine.States;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.LevelsLogic.FinishLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.UI;

namespace _Main._Scripts.Level.StateMachine
{
    public class LevelStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;
        private readonly Saves _saves;
        private readonly LevelService _levelService;
        private readonly MainConfig _mainConfig;
        private readonly UILocator _uiLocator;
        private readonly CameraService _cameraService;
        private readonly SoldiersPool _soldiersPool;

        public LevelStateMachine(Saves saves, LevelService levelService, MainConfig mainConfig,
            List<CellToMerge> reserveCells, List<CellToMerge> gameCells, UILocator uiLocator,
            CameraService cameraService,SoldiersPool soldiersPool,Player player)
        {
            _saves = saves;
            _levelService = levelService;
            _mainConfig = mainConfig;
            _uiLocator = uiLocator;
            _cameraService = cameraService;
            _soldiersPool = soldiersPool;
            var preGameView = _uiLocator.GetViewByType<PreGameView>();
            var gameView = _uiLocator.GetViewByType<GameView>();
            var gameOverView = _uiLocator.GetViewByType<GameOverView>();
            var finishView = _uiLocator.GetViewByType<FinishView>();
            _states = new List<IState>
            {
                new InitState(this, levelService, saves,player),
                new MergeState(this, mainConfig,soldiersPool, reserveCells, gameCells, preGameView, _cameraService,saves),
                new PlayState(this,gameView,soldiersPool,_cameraService,_saves,player,mainConfig.Soldiers,mainConfig.UpgradeConfig,levelService),
                new GameOverState(this,saves,gameOverView,player),
                new FinishState(this,finishView,cameraService,saves,levelService,player)
            };

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void SwitchState<TState>() where TState : IState
        {
            var state = _states.FirstOrDefault(state => state is TState);

            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void Update() => _currentState.Update();
    }
}