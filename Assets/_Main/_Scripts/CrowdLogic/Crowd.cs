using System;
using System.Collections;
using System.Collections.Generic;
using _Main._Scripts.Boosts;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Soilders;
using _Main._Scripts.Soilders.Bullets;
using Kimicu.YandexGames.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.CrowdLogic
{
    public class Crowd
    {
        public UnityEvent<float, float, float> OnTakeBoost = new();
        public UnityEvent<int> OnTakeMoney = new();
        public UnityEvent OnTakeSoldier = new();
        public UnityEvent<int> OnSoldiersCountChanged = new();
        public List<Soldier> Soldiers { get; } = new();
        private readonly List<Transform> _points;
        private readonly Saves _saves;
        private readonly BulletPool _bulletPool;
        private readonly SoldiersPool _soldiersPool;

        private readonly float _soldierSpeed;
        private readonly float _maxPosition;

        private float _bulletDamagePercentage;
        private float _bulletSpeedPercentage;
        private float _bulletScalePercentage = 100;
        private float _fireRatePercentage;
        private SoldierAnimationTriggers _currentTrigger;

        private readonly List<Soldier> _diedSoldiers = new();
        public int SoldiersCount => Soldiers.Count;
        private int MaxSoldiers => _points.Count;
        private bool _doubleShotIsActive;

        public Crowd(List<Transform> points, PlayerConfig config, BulletPool bulletPool, SoldiersPool soldiersPool,
            Saves saves)
        {
            _points = points;
            _saves = saves;
            _bulletPool = bulletPool;
            _soldiersPool = soldiersPool;
            _maxPosition = config.soldiersMaxPosition;
            _soldierSpeed = config.soldierSpeed;
        }

        public void UpdateSoldiers()
        {
            MoveSoldiersTowardsPoints();
            ClampSoldiersPositionX();
            UpdateShootingCooldownForAllSoldiers();
        }


        public void ResetBoostsPercentages(float bulletDamagePercentage, float bulletSpeedPercentage,
            float fireRatePercentage)
        {
            _bulletDamagePercentage = bulletDamagePercentage;
            _bulletSpeedPercentage = bulletSpeedPercentage;
            _fireRatePercentage = fireRatePercentage;
            _doubleShotIsActive = false;
            _bulletScalePercentage = 100;
        }

        public void UpdateShootingCooldownForAllSoldiers()
        {
            foreach (var soldier in Soldiers)
                soldier.UpdateShootingCooldown();
        }

        private void UpdateBulletBoostPercentages(Boost boost)
        {
            switch (boost.Type)
            {
                case BoostType.Damage:
                    _bulletDamagePercentage += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletDamagePercentage(100 + boost.Value);
                    break;
                case BoostType.BulletScale:
                    _bulletScalePercentage += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletScalePercentage(100 + boost.Value);
                    break;
                case BoostType.BulletSpeed:
                    _bulletSpeedPercentage += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletSpeedPercentage(100 + boost.Value);
                    break;
                case BoostType.FireRate:
                    _fireRatePercentage -= boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateFireRatePercentage(100 - boost.Value);
                    break;
                case BoostType.DoubleBullet:
                    _doubleShotIsActive = true;
                    foreach (var soldier in Soldiers) soldier.ActivateDoubleShot();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnTakeBoost.Invoke(_bulletDamagePercentage, _fireRatePercentage, _bulletSpeedPercentage);
        }

        public void SetAnimationForAllSoldiers(SoldierAnimationTriggers trigger)
        {
            _currentTrigger = trigger;
            foreach (var soldier in Soldiers)
                soldier.SetAnimation(trigger);
        }

        public void SetFinishShootingSettings()
        {
            foreach (var soldier in Soldiers)
            {
                soldier.SetFinishRotation();
                soldier.EnableFinishShooting();
            }

            _bulletPool.ReturnAllBullets();
        }

        //TODO: ВЫнести в отдельный класс

        #region Movement

        private void MoveSoldiersTowardsPoints()
        {
            for (int i = 0; i < Soldiers.Count; i++)
            {
                Soldiers[i].SetLookDirection(_points[i].position);

                Soldiers[i].transform.position = Vector3.Lerp(Soldiers[i].transform.position, _points[i].position,
                    Time.deltaTime * _soldierSpeed);
            }
        }

        private void ClampSoldiersPositionX()
        {
            foreach (var soldier in Soldiers)
            {
                var clamp = Mathf.Clamp(soldier.transform.position.x, -_maxPosition, _maxPosition);
                soldier.transform.position = new Vector3(clamp, soldier.transform.position.y,
                    soldier.transform.position.z);
            }
        }

        #endregion

        public void SaveCurrentSoldiers()
        {
            var soldierForAdd = new List<Saves.Soldier>();
            
            foreach (var soldier in Soldiers)
            {
                if (soldier.cellIndex <= _saves.MaxGameCellsCount) soldierForAdd.Add(new Saves.Soldier(soldier.Level, soldier.cellIndex));
                else _saves.TrySaveInReserve(soldier.Level);
            }
            _saves.InGameSoldiers.Clear();
            _saves.InGameSoldiers.AddRange(soldierForAdd);
        }


        public void ResetCrowd()
        {
            foreach (var soldier in Soldiers)
            {
                soldier.onDie.RemoveAllListeners();
                soldier.onTouchSoldier.RemoveAllListeners();
                soldier.onTouchBoost.RemoveAllListeners();
                soldier.onTouchMoney.RemoveAllListeners();
                _soldiersPool.ReturnSoldier(soldier);
            }

            Soldiers.Clear();

            foreach (var soldier in _diedSoldiers)
                _soldiersPool.ReturnSoldier(soldier);
            _diedSoldiers.Clear();

            _saves.InvokeSave();
        }

        public void AddToCrowdAndSetPosition(Soldier soldier,int cellIndex)
        {
            soldier.cellIndex = cellIndex;
            AddToCrowd(soldier, false);
            var index = Soldiers.IndexOf(soldier);
            soldier.transform.position = _points[index].position;
        }

        private void AddToCrowd(Soldier soldier)
        {
            if (SoldiersCount >= MaxSoldiers) return;
            AddToCrowd(soldier, false, 0);
            OnTakeSoldier.Invoke();
        }

        private void AddToCrowd(Soldier soldier, bool setAtPosition = false, int atPosition = 0)
        {
            soldier.InvitedToCrowd(_bulletPool, _bulletDamagePercentage, _bulletSpeedPercentage,
                _bulletScalePercentage, _fireRatePercentage, _doubleShotIsActive);

            if (setAtPosition) Soldiers.Insert(atPosition, soldier);
            else Soldiers.Add(soldier);

            soldier.onDie.AddListener(RemoveFromCrowd);
            soldier.onTouchSoldier.AddListener(AddToCrowd);
            soldier.onTouchBoost.AddListener(UpdateBulletBoostPercentages);
            soldier.onTouchMoney.AddListener(value=>
            {
                _saves.AddMoney(value);
                OnTakeMoney.Invoke(value);
            });
            soldier.SetAnimation(_currentTrigger);
            OnSoldiersCountChanged.Invoke(SoldiersCount);
        }

 

        private void RemoveFromCrowd(Soldier soldier)
        {
            soldier.onDie.RemoveAllListeners();
            soldier.onTouchSoldier.RemoveAllListeners();
            soldier.onTouchBoost.RemoveAllListeners();
            soldier.onTouchMoney.RemoveAllListeners();
            var configSoldiersLevel = soldier.Config.SoldiersLevel;

            if (configSoldiersLevel <= SoldiersLevels.Level1)
            {
                Soldiers.Remove(soldier);
                Coroutines.StartRoutine(WaitToReturnSoldier(soldier, 3f));
                _diedSoldiers.Add(soldier);
                return;
            }

            OnSoldiersCountChanged.Invoke(SoldiersCount);
            DownGradeSoldier(soldier);
        }

        private void DownGradeSoldier(Soldier soldier)
        {
            var newSoldier = _soldiersPool.GetSoldierFromLevel<Soldier>(soldier.Level - 1);
            newSoldier.cellIndex = soldier.cellIndex;
            newSoldier.transform.position = soldier.transform.position;
            newSoldier.transform.rotation = soldier.transform.rotation;

            var index = Soldiers.IndexOf(soldier);
            Soldiers.Remove(soldier);

            AddToCrowd(newSoldier, true, index);
            Coroutines.StartRoutine(WaitToReturnSoldier(soldier, 3f));
        }


        private IEnumerator WaitToReturnSoldier(Soldier soldier, float delay)
        {
            yield return new WaitForSeconds(delay);
            _soldiersPool.ReturnSoldier(soldier);
        }
    }
}