using System;
using System.Collections.Generic;
using _Main._Scripts.CrowdLogic;
using _Main._Scripts.Level.StateMachine.States;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.MergeLogic.DragAndDropLogic;
using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using _Main._Scripts.Services.Cameras;
using _Main._Scripts.TutorialLogic;
using _Main._Scripts.UI;
using _Main._Scripts.UpgradeLogic;
using Kimicu.YandexGames;
using SoundService.Data;
using SoundService.Scripts;
using CameraType = _Main._Scripts.Services.Cameras.CameraType;
using IState = _Main._Scripts.PlayerLogic.StateMachine.States.IState;

namespace _Main._Scripts.LevelsLogic.StateMachine.States
{
    public class MergeState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly MainConfig _mainConfig;
        private readonly SoldiersPool _soldiersPool;
        private readonly List<CellToMerge> _reserveCells;
        private readonly List<CellToMerge> _gameCells;
        private readonly PreGameView _preGameView;
        private readonly CameraService _cameraService;
        private readonly Saves _saves;


        private DragAndDrop _dragAndDrop;
        private CellToMerge _startDragCell;
        private CellToMerge _selectedCell;
        private RepresentativeOfTheSoldiers _representativeOfTheSoldiers;

        private SoldiersLevels _currentRewardSoldier;
        private bool _isFirstLaunch = true;
        private readonly TutorialService _tutorialService;
        private readonly AudioService _audioService;

        private const int MaxLoseStreakCountForShowPopUp = 5;

        public MergeState(IStateSwitcher stateSwitcher, MainConfig mainConfig, SoldiersPool soldiersPool,
            List<CellToMerge> reserveCells,
            List<CellToMerge> gameCells, PreGameView preGameView, CameraService cameraService, Saves saves)
        {
            _stateSwitcher = stateSwitcher;
            _mainConfig = mainConfig;
            _soldiersPool = soldiersPool;
            _reserveCells = reserveCells;
            _gameCells = gameCells;
            _preGameView = preGameView;
            _cameraService = cameraService;
            _saves = saves;
            _representativeOfTheSoldiers = new RepresentativeOfTheSoldiers(soldiersPool);

            _dragAndDrop = new DragAndDrop(mainConfig.DragConfig, _cameraService.Holder.MainCamera,
                _representativeOfTheSoldiers, soldiersPool);

            UpdateUpgradeView(_preGameView.DamageUpgradeCell, _saves.BulletDamageLevel,
                _mainConfig.UpgradeConfig.DamageUpgradeRatios, true);
            UpdateUpgradeView(_preGameView.FireRateUpgradeCell, _saves.FireRateLevel,
                _mainConfig.UpgradeConfig.FireRateUpgradeRatios, true);
            UpdateUpgradeView(_preGameView.BulletSpeedUpgradeCell, _saves.BulletSpeedLevel,
                _mainConfig.UpgradeConfig.SpeedUpgradeRatios, true);

            _preGameView.DamageUpgradeCell.BuyButton.onClick.AddListener(() =>
                UpgradePlayer(mainConfig.UpgradeConfig.DamageUpgradeRatios,
                    ref _saves.BulletDamageLevel, ref _saves.BulletDamagePercentage,
                    () => UpdateUpgradeView(_preGameView.DamageUpgradeCell, _saves.BulletDamageLevel,
                        _mainConfig.UpgradeConfig.DamageUpgradeRatios)
                ));

            _preGameView.BulletSpeedUpgradeCell.BuyButton.onClick.AddListener(() =>
                UpgradePlayer(mainConfig.UpgradeConfig.SpeedUpgradeRatios, ref _saves.BulletSpeedLevel,
                    ref _saves.BulletSpeedPercentage, () => UpdateUpgradeView(_preGameView.BulletSpeedUpgradeCell,
                        _saves.BulletSpeedLevel,
                        _mainConfig.UpgradeConfig.SpeedUpgradeRatios)
                ));

            _preGameView.FireRateUpgradeCell.BuyButton.onClick.AddListener(() =>
                UpgradePlayer(mainConfig.UpgradeConfig.FireRateUpgradeRatios, ref _saves.FireRateLevel,
                    ref _saves.FireRatePercentage, () => UpdateUpgradeView(_preGameView.FireRateUpgradeCell,
                        _saves.FireRateLevel,
                        _mainConfig.UpgradeConfig.FireRateUpgradeRatios)
                ));
            _preGameView.RewardSoldier.onClick.AddListener(TryAddedRewardSoldier);
            _preGameView.StartGameButton.onClick.AddListener(SwitchToPlayState);

            _tutorialService = ServiceLocator.Instance.GetServiceByType<TutorialService>();
            _audioService = ServiceLocator.Instance.GetServiceByType<AudioService>();
        }

        public void Enter()
        {
            _audioService.PlaySound(Sound.MenuMusic,volumeScale:0.05f,loop:true);
            ClearCells(_reserveCells);
            ClearCells(_gameCells);
            LoadSoldiersFromSave();

            if (_saves.LoseStreak >= MaxLoseStreakCountForShowPopUp) _preGameView.ShowRewardProposal();

            _preGameView.LevelText.SetValue(_saves.CurrentLevelText + 1);

            _cameraService.ShowFade(() =>
            {
                _cameraService.SwitchToFromType(CameraType.PreGame);
                _cameraService.HideFade(() =>
                {
                    _preGameView.Open();
                    _tutorialService.TryCallNextStep();
                }, _isFirstLaunch);
            });

            SetCurrentRewardSoldier();

            _saves.InvokeSave();
        }

        public void Exit()
        {
            if (_saves.WasShowedTutorial == false && _isFirstLaunch && _saves.CurrentLevelText == 0)
            {
                //TODO:здесь можно дюпать солдат если на этапе туториала с мержем не проходиь его ,а перезагружать страницу,так как это маловероятно не буду править 
                for (int i = 0; i < 2; i++)
                {
                    var index = TryGetFreeIndexInList(_reserveCells);
                    var soldier = _representativeOfTheSoldiers.GetSoldier(SoldiersLevels.Level1);
                    _reserveCells[(int)index].AddObject(soldier);
                }

                SaveSoldiers();
            }
            else
                SaveSoldiers();

            _preGameView.Close();
            _isFirstLaunch = false;
        }

        public void Update() => _dragAndDrop.UpdateDrag();

        private void SetCurrentRewardSoldier()
        {
            if (_saves.CurrentLevelText < 5)
                _currentRewardSoldier = SoldiersLevels.Level5;
            else if (_saves.CurrentLevelText < 9)
                _currentRewardSoldier = SoldiersLevels.Level7;
            else if (_saves.CurrentLevelText < 14)
                _currentRewardSoldier = SoldiersLevels.Level9;
            else if (_saves.CurrentLevelText < 19)
                _currentRewardSoldier = SoldiersLevels.Level11;
            else if (_saves.CurrentLevelText >= 19)
                _currentRewardSoldier = SoldiersLevels.Level13;
            _preGameView.SoldierRewardText.SetValue((int)_currentRewardSoldier + 1);
        }

        private void SwitchToPlayState()
        {
            var soldiersCount = 0;
            foreach (var cell in _gameCells)
                if (cell.IsBusy)
                    soldiersCount++;

            if (soldiersCount <= 0) return;
            _stateSwitcher.SwitchState<PlayState>();
        }

        private void TryAddedRewardSoldier()
        {
            var index = TryGetFreeIndexInList(_reserveCells);
            var isReserveCells = true;
            if (index == null)
            {
                index = TryGetFreeIndexInList(_gameCells);
                isReserveCells = false;
            }

            if (index == null) return;
            //TODO: Логика получения отличается от того, что я делал раньше. Наверное надо будет сделать по другому
            Advertisement.ShowVideoAd(() => Audio.MuteAllAudio(), () =>
            {
                var soldier = _representativeOfTheSoldiers.GetSoldier(_currentRewardSoldier);
                if (isReserveCells) _reserveCells[(int)index].AddObject(soldier, true);
                else _gameCells[(int)index].AddObject(soldier, true);
                
                YandexMetrika.Event("RewardSoldier");
                SaveSoldiers();
            }, () => Audio.UnMuteAllAudio());
        }

        private int? TryGetFreeIndexInList(List<CellToMerge> list)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].IsBusy == false)
                    return i;

            return null;
        }

        private void UpdateUpgradeView(UpgradePanelCell upgradeCell, int level, List<UpgradeData> upgradeRatios,
            bool firstCall = false)
        {
            if (level >= upgradeRatios.Count)
            {
                upgradeCell.SetMaxLevel();
                upgradeCell.TextAnimation.StartAnimation();
                return;
            }

            upgradeCell.UpdateCellTexts(level, upgradeRatios[level].Cost, upgradeRatios[level].Percentage, firstCall);
        }

        private void ClearCells(List<CellToMerge> cells)
        {
            foreach (var cell in cells)
                if (cell.IsBusy)
                {
                    _soldiersPool.ReturnSoldier(cell.currentObject);
                    cell.RemoveObjectData();
                }
        }

        private void LoadSoldiersFromSave()
        {
            MainLoadSoldiers(_gameCells, _saves.InGameSoldiers);
            MainLoadSoldiers(_reserveCells, _saves.ReserveSoldiers);
        }

        private void SaveSoldiers()
        {
            MainSaveSoldiers(_gameCells, _saves.InGameSoldiers);
            MainSaveSoldiers(_reserveCells, _saves.ReserveSoldiers);
        }

        private void MainLoadSoldiers(List<CellToMerge> cells, List<Saves.Soldier> listFromSave)
        {
            foreach (var soldier in listFromSave)
            {
                var instSoldier = _representativeOfTheSoldiers.GetSoldier(soldier.Level);
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

        private void UpgradePlayer(IReadOnlyList<UpgradeData> upgradeData, ref int upgradeLevel,
            ref float upgradePercentage,
            Action successCallback = null)
        {
            var cost = upgradeData[upgradeLevel].Cost;
            if (_saves.TrySpendMoney(cost))
            {
                upgradePercentage += upgradeData[upgradeLevel].Percentage;
                if (upgradeLevel <= upgradeData.Count - 1)
                {
                    upgradeLevel++;
                }

                successCallback?.Invoke();
            }
        }
    }
}