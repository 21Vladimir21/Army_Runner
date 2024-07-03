using System.Collections.Generic;
using _Main._Scripts.Boosts;
using _Main._Scripts.MergeLogic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.Soilders;
using _Main._Scripts.Soilders.Bullets;
using UnityEngine;

namespace _Main._Scripts.CrowdLogic
{
    public class Crowd
    {
        public List<Soldier> Soldiers { get; private set; } = new();
        private readonly List<Transform> _points;
        private readonly Soldiers _soldiers;
        private readonly BulletPool _bulletPool;

        private readonly float _soldierSpeed;
        private readonly float _maxPosition;

        private float _bulletDamageRatio;
        private float _bulletSpeedRatio;
        private float _bulletScaleRatio = 1f;
        private float _fireRateRatio;

        private List<GameObject> _diedSoldiers = new();
        public int SoldiersCount => Soldiers.Count;

        public Crowd(List<Transform> points, PlayerConfig config, BulletPoolConfig bulletPoolConfig, Soldiers soldiers,
            float bulletDamageRatio, float bulletSpeedRatio, float fireRateRatio)
        {
            _points = points;
            _soldiers = soldiers;
            _bulletPool = new BulletPool(bulletPoolConfig);
            _maxPosition = config.soldiersMaxPosition;
            _soldierSpeed = config.soldierSpeed;

            ResetBoostsRatios(bulletDamageRatio, bulletSpeedRatio, fireRateRatio);
        }

        public void ResetBoostsRatios(float bulletDamageRatio, float bulletSpeedRatio, float fireRateRatio)
        {
            _bulletDamageRatio = bulletDamageRatio;
            _bulletSpeedRatio = bulletSpeedRatio;
            _fireRateRatio = fireRateRatio;
        }


        public void UpdateSoldiers()
        {
            MoveSoldiersTowardsPoints();
            ClampSoldiersPositionX();
            UpdateShootingCooldownForAllSoldiers();
        }

        public void UpdateBulletBoostRatio(Boost boost)
        {
            switch (boost.Type)
            {
                case BoostType.Damage:
                    _bulletDamageRatio += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletDamageRatio(_bulletDamageRatio);
                    break;
                case BoostType.BulletScale:
                    _bulletScaleRatio += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletScaleRatio(_bulletScaleRatio);
                    break;
                case BoostType.BulletSpeed:
                    _bulletSpeedRatio += boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateBulletSpeedRatio(_bulletSpeedRatio);
                    break;
                case BoostType.FireRate:
                    _fireRateRatio -= boost.Value;
                    foreach (var soldier in Soldiers) soldier.UpdateFireRateRatio(_fireRateRatio);
                    break;
                case BoostType.DoubleBullet:
                    foreach (var soldier in Soldiers) soldier.ActivateDoubleShot();
                    break;
            }
        }

        public void UpdateShootingCooldownForAllSoldiers()
        {
            foreach (var soldier in Soldiers)
                soldier.UpdateShootingCooldown();
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
            AddToCrowd(soldier);
            var index = Soldiers.IndexOf(soldier);
            soldier.transform.position = _points[index].position;
        }

        public int AddToCrowd(Soldier soldier, bool setAtPosition = false, int atPosition = 0)
        {
            soldier.InvitedToCrowd(_bulletPool, _bulletDamageRatio, _bulletSpeedRatio, _bulletScaleRatio,
                _fireRateRatio);
            
            if (setAtPosition)
                Soldiers.Insert(atPosition, soldier);
            else
                Soldiers.Add(soldier);
            
            soldier.onDie.AddListener(RemoveFromCrowd);
            return Soldiers.IndexOf(soldier);
        }

        public void ResetCrowd()
        {
            foreach (var soldier in Soldiers)
                Object.Destroy(soldier.gameObject);
            Soldiers.Clear();

            foreach (var soldier in _diedSoldiers)
                Object.Destroy(soldier);
            _diedSoldiers.Clear();
        }

        private void RemoveFromCrowd(Soldier soldier)
        {
            var configSoldiersLevel = soldier.Config.SoldiersLevel;
            if (configSoldiersLevel <= SoldiersLevels.Level1)
            {
                Soldiers.Remove(soldier);
                _diedSoldiers.Add(soldier.gameObject);
                return;
            }

            DownGradeSoldier(soldier, configSoldiersLevel);
        }

        private void DownGradeSoldier(Soldier soldier, SoldiersLevels configSoldiersLevel)
        {
            var newSoldier = Object.Instantiate(_soldiers.GetSoldierFromLevel(configSoldiersLevel - 1)
                , soldier.transform.position, soldier.transform.rotation);

            var index = Soldiers.IndexOf(soldier);
            Soldiers.Remove(soldier);
            Object.Destroy(soldier.gameObject); //TODO: Добавить пул солдат 
            AddToCrowd(newSoldier, true, index);
        }
    }
}