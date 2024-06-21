using System.Collections.Generic;
using System.Linq;
using _Main._Scripts.Level.StateMachine.States;
using _Main._Scripts.LevelsLogic;
using _Main._Scripts.LevelsLogic.StateMachine.States;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.Player.StateMachine;
using _Main._Scripts.Player.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.UI;
using UnityEngine;

namespace _Main._Scripts.Level.StateMachine
{
    public class LevelStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;
        private readonly Saves _saves;
        private readonly LevelSpawner _levelSpawner;
        private readonly MainConfig _mainConfig;
        private readonly UILocator _uiLocator;

        public LevelStateMachine(Saves saves, LevelSpawner levelSpawner, MainConfig mainConfig,
            List<CellToMerge> reserveCells, List<CellToMerge> gameCells,UILocator uiLocator,Camera camera)
        {
            _saves = saves;
            _levelSpawner = levelSpawner;
            _mainConfig = mainConfig;
            _uiLocator = uiLocator;
            var preGameView = _uiLocator.GetViewByType<PreGameView>();
            _states = new List<IState>
            {
                new MergeState(this,mainConfig.DragConfig,reserveCells,gameCells,preGameView,camera),
                new InitState(this, levelSpawner, saves, mainConfig.LevelsConfig),
                new PlayState(),
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