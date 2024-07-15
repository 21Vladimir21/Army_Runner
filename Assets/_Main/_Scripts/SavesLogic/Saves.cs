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
        [field: SerializeField] public int CurrentLevel { get; private set; }
        [field: SerializeField] public int CurrentLevelText { get; private set; }

        [field: SerializeField] public bool SoundEnabled { get; set; } = true;
        [field: SerializeField] public bool AdEnabled { get; set; } = true;

        public bool CanShowAd => CurrentLevel > LastLevelNotShowAd;

        [field: SerializeField]
        public List<Soldier> InGameSoldiers { get; set; } = new()
        {
            new Soldier(SoldiersLevels.Level1, 1),
        };

        private const int LastLevelNotShowAd = 5;
        public int MaxReserveCapacity = 24;
        [field: SerializeField] public List<Soldier> ReserveSoldiers { get; set; } = new(24);


        [field: SerializeField] public int BulletDamageLevel;
        [field: SerializeField] public int BulletSpeedLevel;
        [field: SerializeField] public int FireRateLevel;

        [field: SerializeField] public float BulletDamagePercentage = 100;
        [field: SerializeField] public float BulletSpeedPercentage = 100;
        [field: SerializeField] public float FireRatePercentage = 100;

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
            // if (CurrentLevel % 55 == 0)
            //     CurrentLevel = 25;
//TODO:Раскомментировать и правильно указать уровень для зацикливания
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