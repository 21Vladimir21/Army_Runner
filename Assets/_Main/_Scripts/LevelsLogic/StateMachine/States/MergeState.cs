using System.Collections.Generic;
using System.Linq;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.MergeLogic.DragAndDropLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.Soilders;
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
        private readonly Saves _saves;

        private List<DraggableObject> _soldiersPrefabs;

        private DragAndDrop _dragAndDrop;
        private CellToMerge _startDragCell;
        private CellToMerge _cell;

        public MergeState(IStateSwitcher stateSwitcher, DragConfig dragConfig, List<CellToMerge> reserveCells,
            List<CellToMerge> gameCells, PreGameView preGameView, CameraService cameraService, Saves saves)
        {
            _stateSwitcher = stateSwitcher;
            _dragConfig = dragConfig;
            _reserveCells = reserveCells;
            _gameCells = gameCells;
            _preGameView = preGameView;
            _cameraService = cameraService;
            _saves = saves;
            _soldiersPrefabs = dragConfig.SoldiersPrefabs;

            _dragAndDrop = new DragAndDrop(dragConfig, _cameraService.Holder.MainCamera);
            _dragAndDrop.OnUpObject.AddListener(SetCurrentDragObject);
            _dragAndDrop.OnSelectNewObject.AddListener(SetCurrentCell);
            _dragAndDrop.OnMouseUp.AddListener(ResetDragData);
        }

        public void Enter()
        {
            LoadSoldiersFromSave();

            _preGameView.StartGameButton.onClick.AddListener(SwitchToPlayState);
            _preGameView.Open();
            _cameraService.SwitchToFromType(CameraType.PreGame);
        }

        public void Exit()
        {
            SaveSoldiersInSave();
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

        private void LoadSoldiersFromSave()
        {
            MainLoadSoldiers(_gameCells, _saves.InGameSoldiers);
            MainLoadSoldiers(_reserveCells, _saves.ReserveSoldiers);
        }

        private void SaveSoldiersInSave()
        {
            MainSaveSoldiers(_gameCells, _saves.InGameSoldiers);
            MainSaveSoldiers(_reserveCells, _saves.ReserveSoldiers);
        }

        private void MainLoadSoldiers(List<CellToMerge> cells, List<SoldiersLevels> saveList)
        {
            for (int i = 0; i <= cells.Count; i++)
            {
                if (saveList.Count <= i) break;
                var soldier = SpawnSoldier(saveList[i]);
                cells[i].AddObject(soldier);
            }
        }

        private void MainSaveSoldiers(List<CellToMerge> cells, List<SoldiersLevels> saveList)
        {
            var soldierForAdd = new List<SoldiersLevels>();
            foreach (var cell in cells)
                if (cell.IsBusy)
                    soldierForAdd.Add(cell.currentObject.Level);

            saveList.Clear();
            saveList.AddRange(soldierForAdd);
        }

        private DraggableObject SpawnNextObjectLevel(SoldiersLevels level) => SpawnSoldier(level + 1);

        private DraggableObject SpawnSoldier(SoldiersLevels level)
        {
            var soldierPrefab = _soldiersPrefabs.FirstOrDefault(x => x.Level == level);
            var soldier = Object.Instantiate(soldierPrefab);
            return soldier;
        }

        private void ResetDragData()
        {
            {
                if (_cell)
                {
                    TryMergeObjects();
                    _cell = null;
                }
                _startDragCell = null;
            }
        }

        private void SetCurrentCell(CellToMerge cell) => _cell = cell;
        private void SetCurrentDragObject(CellToMerge startDragCell) => _startDragCell = startDragCell;
    }
}