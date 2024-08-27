using System;
using System.Collections.Generic;
using System.IO;
using _Main._Scripts.MergeLogic;
using Kimicu.YandexGames;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.SavesLogic
{
    [Serializable]
    public class Saves
    {
        [SerializeField] private int _money;
        [field: SerializeField] public int CurrentLevel { get; set; }
        [field: SerializeField] public int CurrentLevelText { get; private set; }
        [field: SerializeField] public bool SoundEnabled { get; set; } = true;
        [field: SerializeField] public bool MusicEnabled { get; set; } = true;
        [field: SerializeField] public bool AdEnabled { get; set; } = true;

        [field: SerializeField] public int BulletDamageLevel;
        [field: SerializeField] public int BulletSpeedLevel;
        [field: SerializeField] public int FireRateLevel;

        [field: SerializeField] public float BulletDamagePercentage = 100;
        [field: SerializeField] public float BulletSpeedPercentage = 100;
        [field: SerializeField] public float FireRatePercentage = 100;

        [SerializeField] private bool _wasShowedTutorial;
        [field: SerializeField] public int TutorialStepIndex { get; set; }

        [field: SerializeField]
        public List<Soldier> InGameSoldiers { get; set; }
            = new() { new Soldier(SoldiersLevels.Level3, 0), };

        [field: SerializeField] public List<Soldier> ReserveSoldiers { get; set; } = new(24);

        public bool CanShowAd => CurrentLevelText > LastLevelNotShowAd;

        private const int LastLevelNotShowAd = 5;
        [NonSerialized] public int MaxReserveCapacity = 24;
        [NonSerialized] public int MaxGameCellsCount = 11;
        [NonSerialized] public int LoseStreak;

        public bool WasShowedTutorial
        {
            get => _wasShowedTutorial;
            set
            {
                _wasShowedTutorial = value;
                InvokeSave();
            }
        }

        private string _filePath;

        public int Money
        {
            get => _money;
            private set
            {
                _money = value;
                OnMoneyChanged?.Invoke();
            }
        }

        public event UnityAction OnMoneyChanged;

        #region Save

        public void LoadSaves()
        {
        }

        public void InvokeSave(bool forcedToDo = false)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetCloudSaveData();
#endif
#if UNITY_WEBGL && UNITY_EDITOR
            SetLocalSaveData();
#endif
        }

        private void SetLocalSaveData()
        {
            _filePath = GetFilePath();

            var json = JsonUtility.ToJson(this, true);
            File.WriteAllText(_filePath, json);
        }

        private void SetCloudSaveData()
        {
            var json = JsonUtility.ToJson(this, true);
            Cloud.SetValue(SaveKey, json, true, () => Debug.Log("Save success \n" + json),
                (log) => Debug.Log("Save error \n" + log));
        }

        public static string SaveKey => "saves";

        public static string GetFilePath()
        {
            return Path.Combine(Application.dataPath, "save.json");
        }

        #endregion

        #region Purchases.NoAd.BuyAll

        public void BuyNoAd()
        {
            AdEnabled = false;
            InvokeSave();
        }

        #endregion

        #region Level

        public void SetNextLevel()
        {
            CurrentLevel++;
            CurrentLevelText++;
            
            if (CurrentLevel <= 15)
                YandexMetrika.Event($"Lvl{CurrentLevel}");


            if (CurrentLevel % 42 == 0)
                CurrentLevel = 29;

            InvokeSave();
        }

        public void SetPreviousLevel()
        {
            CurrentLevel--;
            CurrentLevelText--;
            InvokeSave();
        }

        #endregion

        #region Money

        public void AddMoney(int amount)
        {
            if (amount <= 0)
                return;

            Money += amount;
            InvokeSave();
        }

        public bool TrySpendMoney(int amount)
        {
            if (!CanSpendMoney(amount)) return false;

            Money -= amount;
            InvokeSave();

            return true;
        }

        public bool CanSpendMoney(int amount) => amount > 0 && amount <= Money;

        #endregion

        #region PlayerUpgrade

        #endregion


        public int TrySaveInReserve(SoldiersLevels level)
        {
            for (var i = 0; i < MaxReserveCapacity; i++)
            {
                var indexFound = false;
                foreach (var reserveSoldier in ReserveSoldiers)
                {
                    if (reserveSoldier.Index == i)
                    {
                        indexFound = true;
                        break;
                    }
                }

                if (indexFound) continue;
                ReserveSoldiers.Add(new Saves.Soldier(level, i));
                return i;
            }

            InvokeSave();
            return default;
        }

        [Serializable]
        public struct Soldier
        {
            public SoldiersLevels Level;
            public int Index;

            public Soldier(SoldiersLevels level, int index)
            {
                Level = level;
                Index = index;
            }
        }
    }
}