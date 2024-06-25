using System;
using System.Collections.Generic;
using System.IO;
using _Main._Scripts.MergeLogic;
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
        [field: SerializeField] public List<SoldiersLevels> InGameSoldiers= new();

        [field: SerializeField] public List<SoldiersLevels> ReserveSoldiers = new()
        {
            SoldiersLevels.Level1, SoldiersLevels.Level1,SoldiersLevels.Level3, SoldiersLevels.Level1, SoldiersLevels.Level2,
        };


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
            // Cloud.SetValue(SaveKey, json, true, () => Debug.Log("Save success \n" + json),
            // (log) => Debug.Log("Save error \n" + log));
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
            if (CurrentLevel % 55 == 0)
                CurrentLevel = 25;

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
    }
}