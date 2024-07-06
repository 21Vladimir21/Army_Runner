using System;
using System.Collections.Generic;
using _Main._Scripts.Boosts;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Soilders;
using _Main._Scripts.Soilders.Bullets;
using UnityEngine;

namespace _Main._Scripts.CrowdLogic
{
    public class Crowd
    {
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
        }

        public void UpdateShootingCooldownForAllSoldiers()
        {
            foreach (var soldier in Soldiers)
                soldier.UpdateShootingCooldown();
        }

        public void UpdateBulletBoostPercentages(Boost boost)
        {
            switch (boost.Type)
            {
                case BoostType.Damage:
                    _bulletDamagePercentage += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletDamagePercentage(_bulletDamagePercentage);
                    break;
                case BoostType.BulletScale:
                    _bulletScalePercentage += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletScalePercentage(_bulletScalePercentage);
                    break;
                case BoostType.BulletSpeed:
                    _bulletSpeedPercentage += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletSpeedPercentage(_bulletSpeedPercentage);
                    break;
                case BoostType.FireRate:
                    _fireRatePercentage -= boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateFireRatePercentage(_fireRatePercentage);
                    break;
                case BoostType.DoubleBullet:
                    foreach (var soldier in Soldiers) soldier.ActivateDoubleShot();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetAnimationForAllSoldiers(SoldierAnimationTriggers trigger)
        {
            _currentTrigger = trigger;
            foreach (var soldier in Soldiers)
                soldier.SetAnimation(trigger);
        }

        public void SetFinishShootingRotation()
        {
            foreach (var soldier in Soldiers) soldier.SetFinishRotation();
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

        public void AddToCrowdAndSetPosition(Soldier soldier)
        {
            AddToCrowd(soldier, false, 0);
            var index = Soldiers.IndexOf(soldier);
            soldier.transform.position = _points[index].position;
        }

        public void AddToCrowd(Soldier soldier)
        {
            if (SoldiersCount>= MaxSoldiers) return;
            AddToCrowd(soldier, false, 0);
            SaveInReserve(soldier);
        }

        private void AddToCrowd(Soldier soldier, bool setAtPosition = false, int atPosition = 0)
        {
            soldier.InvitedToCrowd(_bulletPool, _bulletDamagePercentage, _bulletSpeedPercentage,
                _bulletScalePercentage, _fireRatePercentage);

            if (setAtPosition) Soldiers.Insert(atPosition, soldier);
            else Soldiers.Add(soldier);

            soldier.onDie.AddListener(RemoveFromCrowd);
            soldier.onTouchSoldier.AddListener(AddToCrowd);
            soldier.onTouchBoost.AddListener(UpdateBulletBoostPercentages);
            soldier.SetAnimation(_currentTrigger);
        }

        private void SaveInReserve(Soldier soldier)
        {
            var capacity = _saves.ReserveSoldiers.Capacity;
            for (var i = 0; i < capacity; i++)
            {
                var indexFound = false;
                foreach (var reserveSoldier in _saves.ReserveSoldiers)
                {
                    if (reserveSoldier.Index == i)
                    {
                        indexFound = true;
                        break;
                    }
                }

                if (indexFound) continue;
                _saves.ReserveSoldiers.Add(new Saves.Soldier(soldier.Config.SoldiersLevel, i));
                _saves.InvokeSave(); //TODO: Подумать куда запихнуть сохранение
            }
        }

        public void ResetCrowd()
        {
            foreach (var soldier in Soldiers)
                _soldiersPool.ReturnSoldier(soldier);
            Soldiers.Clear();

            foreach (var soldier in _diedSoldiers)
                _soldiersPool.ReturnSoldier(soldier);
            _diedSoldiers.Clear();
        }

        private void RemoveFromCrowd(Soldier soldier)
        {
            var configSoldiersLevel = soldier.Config.SoldiersLevel;
            if (configSoldiersLevel <= SoldiersLevels.Level1)
            {
                Soldiers.Remove(soldier);
                _diedSoldiers.Add(soldier);
                return;
            }

            DownGradeSoldier(soldier, configSoldiersLevel);
        }

        private void DownGradeSoldier(Soldier soldier, SoldiersLevels configSoldiersLevel)
        {
            var newSoldier = _soldiersPool.GetSoldierFromLevel<Soldier>(configSoldiersLevel - 1);

            newSoldier.transform.position = soldier.transform.position;
            newSoldier.transform.rotation = soldier.transform.rotation;

            var index = Soldiers.IndexOf(soldier);
            Soldiers.Remove(soldier);
            _soldiersPool.ReturnSoldier(soldier);
            AddToCrowd(newSoldier, true, index);
        }
    }
}