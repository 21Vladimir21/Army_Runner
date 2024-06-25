using System.Collections.Generic;
using System.Linq;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.MergeLogic.DragAndDropLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.UI;
using UnityEngine;
using CameraType = _Main._Scripts.Services.Cameras.CameraType;

namespace _Main._Scripts.Level.StateMachine.States
{
    public class MergeState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly DragConfig _dragConfig;
        private readonly List<CellToMerge> _reserveCells;
        private readonly List<CellToMerge> _gameCells;
        private readonly PreGameView _preGameView;
        private readonly CameraService _cameraService;

        private List<DraggableObject> _soldiersPrefabs;

        private DragAndDrop _dragAndDrop;
        private CellToMerge _startDragCell;
        private CellToMerge _cell;

        public MergeState(IStateSwitcher stateSwitcher, DragConfig dragConfig, List<CellToMerge> reserveCells,
            List<CellToMerge> gameCells, PreGameView preGameView, CameraService cameraService)
        {
            _stateSwitcher = stateSwitcher;
            _dragConfig = dragConfig;
            _reserveCells = reserveCells;
            _gameCells = gameCells;
            _preGameView = preGameView;
            _cameraService = cameraService;
            _soldiersPrefabs = dragConfig.SoldiersPrefabs;

            _dragAndDrop = new DragAndDrop(dragConfig, _cameraService.Holder.MainCamera);
            _dragAndDrop.OnUpObject.AddListener(SetCurrentDragObject);
            _dragAndDrop.OnSelectNewObject.AddListener(SetCurrentCell);
            _dragAndDrop.OnMouseUp.AddListener(ResetDragData);
        }

        public void Enter()
        {
            _reserveCells[0].AddObject(SpawnNextObjectLevel(SoldiersLevels.Level1 - 1));
            _reserveCells[2].AddObject(SpawnNextObjectLevel(SoldiersLevels.Level1 - 1));
            _gameCells[2].AddObject(SpawnNextObjectLevel(SoldiersLevels.Level2 - 1));

            _preGameView.StartGameButton.onClick.AddListener(SwitchToPlayState);
            _preGameView.Open();
            _cameraService.SwitchToFromType(CameraType.PreGame);
        }

        public void Exit()
        {
            Debug.Log("ExitInMergeState");
            _preGameView.Close();
        }

        public void Update()
        {
            _dragAndDrop.UpdateDrag();
        }

        private void SwitchToPlayState()
        {
            _stateSwitcher.SwitchState<PlayState>();
            _preGameView.StartGameButton.onClick.RemoveListener(SwitchToPlayState);
        }


        private void TryMergeObjects()
        {
            if (_cell.IsBusy == false) return;

            if (_startDragCell.currentObject.Level == _cell.currentObject.Level)
            {
                var level = _startDragCell.currentObject.Level;

                _cell.DestroyObject();
                _cell.AddObject(SpawnNextObjectLevel(level));
                _startDragCell.DestroyObject();
            }
        }

        private DraggableObject SpawnNextObjectLevel(SoldiersLevels level)
        {
            var soldierPrefab = _soldiersPrefabs.FirstOrDefault(x => x.Level == level + 1);
            var soldier = Object.Instantiate(soldierPrefab);
            return soldier;
        }

        private void ResetDragData()
        {
            if (_cell)
            {
                TryMergeObjects();
                _cell = null;
            }

            _startDragCell = null;
        }

        private void SetCurrentCell(CellToMerge cell) => _cell = cell;
        private void SetCurrentDragObject(CellToMerge startDragCell) => _startDragCell = startDragCell;
    }
}