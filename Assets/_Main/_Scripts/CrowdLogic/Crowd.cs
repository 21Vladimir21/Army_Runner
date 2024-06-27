using System.Collections.Generic;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.Soilders;
using _Main._Scripts.Soilders.Bullets;
using UnityEngine;

namespace _Main._Scripts.CrowdLogic
{
    public class Crowd
    {
        private readonly List<Soldier> _soldiers = new();
        private readonly List<Transform> _points;
        private readonly BulletPool _bulletPool;

        private readonly float _soldierSpeed;
        private readonly float _maxPosition;

        private float _bulletDamageRatio;
        private float _bulletSpeedRatio;
        private float _bulletScaleRatio = 1f;
        public int SoldiersCount => _soldiers.Count;

        public Crowd(List<Transform> points, PlayerConfig config, BulletPoolConfig bulletPoolConfig,
            float bulletDamageRatio, float bulletSpeedRatio)
        {
            _points = points;
            _bulletPool = new BulletPool(bulletPoolConfig);
            _maxPosition = config.soldiersMaxPosition;
            _soldierSpeed = config.soldierSpeed;
            _bulletDamageRatio = bulletDamageRatio;
            _bulletSpeedRatio = bulletSpeedRatio;
        }


        public void UpdateSoldiers()
        {
            MoveSoldiersTowardsPoints();
            ClampSoldiersPositionX();
            UpdateShootingCooldownForAllSoldiers();
        }

        public void UpdateBulletDamageRatio(float damageRatio)
        {
            _bulletDamageRatio += damageRatio;
            foreach (var soldier in _soldiers) soldier.UpdateBulletDamageRatio(_bulletDamageRatio);
        }

        public void UpdateBulletSpeedRatio(float speedRatio)
        {
            _bulletSpeedRatio += speedRatio;
            foreach (var soldier in _soldiers) soldier.UpdateBulletSpeedRatio(_bulletSpeedRatio);
        }

        public void UpdateBulletScaleRatio(float scaleRatio)
        {
            _bulletScaleRatio += scaleRatio;
            foreach (var soldier in _soldiers) soldier.UpdateBulletScaleRatio(_bulletScaleRatio);
        }

        private void UpdateShootingCooldownForAllSoldiers()
        {
            foreach (var soldier in _soldiers) soldier.UpdateShootingCooldown();
        }

        //TODO: ВЫнести в отдельный класс

        #region Movement

        private void MoveSoldiersTowardsPoints()
        {
            for (int i = 0; i < _soldiers.Count; i++)
            {
                var direction = (_points[i].position - _soldiers[i].transform.position).normalized;
                RotateSoldiers(direction, _soldiers[i].transform);

                _soldiers[i].transform.position = Vector3.Lerp(_soldiers[i].transform.position, _points[i].position,
                    Time.deltaTime * _soldierSpeed);
            }
        }

        private void ClampSoldiersPositionX()
        {
            foreach (var soldier in _soldiers)
            {
                var clamp = Mathf.Clamp(soldier.transform.position.x, -_maxPosition, _maxPosition);
                soldier.transform.position = new Vector3(clamp, soldier.transform.position.y,
                    soldier.transform.position.z);
            }
        }

        private void RotateSoldiers(Vector3 direction, Transform soldier)
        {
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            soldier.rotation = targetRotation;
        }

        #endregion

        public void AddToCrowdAndSetPosition(Soldier soldier)
        {
            AddToCrowd(soldier);
            var index = _soldiers.IndexOf(soldier);
            soldier.transform.position = _points[index].position;
        }

        public void AddToCrowd(Soldier soldier)
        {
            soldier.InvitedToCrowd(_bulletPool, _bulletDamageRatio, _bulletSpeedRatio, _bulletScaleRatio);
            _soldiers.Add(soldier);
            soldier.onDie.AddListener(RemoveFromCrowd);
        }

        private void RemoveFromCrowd(Soldier soldier)
        {
            _soldiers.Remove(soldier);
        }
    }
}