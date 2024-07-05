using System;
using System.Collections.Generic;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.Level.StateMachine.States;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.MergeLogic.DragAndDropLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.UI;
using _Main._Scripts.UpgradeLogic;
using CameraType = _Main._Scripts.Services.Cameras.CameraType;
using IState = _Main._Scripts.PlayerLogic.StateMachine.States.IState;

namespace _Main._Scripts.LevelsLogic.StateMachine.States
{
    public class MergeState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly SoldiersPool _soldiersPool;
        private readonly List<CellToMerge> _reserveCells;
        private readonly List<CellToMerge> _gameCells;
        private readonly PreGameView _preGameView;
        private readonly CameraService _cameraService;
        private readonly Saves _saves;


        private DragAndDrop _dragAndDrop;
        private CellToMerge _startDragCell;
        private CellToMerge _cell;

        public MergeState(IStateSwitcher stateSwitcher, MainConfig mainConfig, SoldiersPool soldiersPool,
            List<CellToMerge> reserveCells,
            List<CellToMerge> gameCells, PreGameView preGameView, CameraService cameraService, Saves saves)
        {
            _stateSwitcher = stateSwitcher;
            _soldiersPool = soldiersPool;
            _reserveCells = reserveCells;
            _gameCells = gameCells;
            _preGameView = preGameView;
            _cameraService = cameraService;
            _saves = saves;

            _dragAndDrop = new DragAndDrop(mainConfig.DragConfig, _cameraService.Holder.MainCamera);
            _dragAndDrop.OnUpObject.AddListener(SetCurrentDragObject);
            _dragAndDrop.OnSelectNewObject.AddListener(SetCurrentCell);
            _dragAndDrop.OnMouseUp.AddListener(ResetDragData);

            _preGameView.DamageUpgradeButton.onClick.AddListener(() =>
                UpgradePlayer(mainConfig.UpgradeConfig.DamageUpgradeRatios,
                    ref _saves.BulletDamageLevel, ref _saves.BulletDamagePercentage,
                    () => _preGameView.UpdateBulletDamageLevelText(_saves.BulletDamageLevel)));

            _preGameView.BulletSpeedUpgradeButton.onClick.AddListener(() =>
                UpgradePlayer(mainConfig.UpgradeConfig.SpeedUpgradeRatios, ref _saves.BulletSpeedLevel,
                    ref _saves.BulletSpeedPercentage,
                    () => _preGameView.UpdateBulletSpeedLevelText(_saves.BulletSpeedLevel)));

            _preGameView.FireRateUpgradeButton.onClick.AddListener(() =>
                UpgradePlayer(mainConfig.UpgradeConfig.FireRateUpgradeRatios, ref _saves.FireRateLevel,
                    ref _saves.FireRatePercentage,
                    () => _preGameView.UpdateFireRateLevelText(_saves.FireRateLevel)));
        }

        public void Enter()
        {
            ClearCell(_reserveCells);
            ClearCell(_gameCells);
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

            if (_startDragCell.currentObject.Level != _cell.currentObject.Level) return;
            var level = _startDragCell.currentObject.Level;

            _soldiersPool.ReturnSoldier(_cell.currentObject);
            _soldiersPool.ReturnSoldier(_startDragCell.currentObject);
            _cell.ReturnObject();
            _startDragCell.ReturnObject();
            _cell.AddObject(GetNextObjectLevel(level));
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

        private void MainLoadSoldiers(List<CellToMerge> cells, List<Saves.Soldier> listFromSave)
        {
            foreach (var soldier in listFromSave)
            {
                var instSoldier = GetSoldier(soldier.Level);
                cells[soldier.Index].AddObject(instSoldier);
            }
        }

        private void MainSaveSoldiers(List<CellToMerge> cells, List<Saves.Soldier> listFromSave)
        {
            var soldierForAdd = new List<Saves.Soldier>();

            for (int i = 0; i < cells.Count; i++)
                if (cells[i].IsBusy)
                    soldierForAdd.Add(new Saves.Soldier(cells[i].currentObject.Level, i));

            listFromSave.Clear();
            listFromSave.AddRange(soldierForAdd);
        }

        private void ClearCell(List<CellToMerge> cells)
        {
            foreach (var cell in cells)
                if (cell.IsBusy)
                {
                    _soldiersPool.ReturnSoldier(cell.currentObject);
                    cell.ReturnObject();
                }
        }

        private DraggableObject GetNextObjectLevel(SoldiersLevels level) => GetSoldier(level + 1);

        private DraggableObject GetSoldier(SoldiersLevels level) =>
            _soldiersPool.GetSoldierFromLevel<DraggableObject>(level);

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

        private void UpgradePlayer(IReadOnlyList<UpgradeData> upgradeData, ref int upgradeLevel,
            ref float upgradePercentage,
            Action successCallback = null)
        {
            var cost = upgradeData[upgradeLevel].Cost;
            if (_saves.TrySpendMoney(cost))
            {
                upgradePercentage += upgradeData[upgradeLevel].Percentage;
                upgradeLevel++;
                successCallback?.Invoke();
            }
        }
    }
}