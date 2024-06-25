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
        public int SoldiersCount => _soldiers.Count;

        public Crowd(List<Transform> points, PlayerConfig config, BulletPoolConfig bulletPoolConfig)
        {
            _points = points;
            _bulletPool = new BulletPool(bulletPoolConfig);
            _maxPosition = config.soldiersMaxPosition;
            _soldierSpeed = config.soldierSpeed;
        }


        public void UpdateSoldiers()
        {
            MoveSoldiersTowardsPoints();
            ClampSoldiersPositionX();
            UpdateShootingCooldownForAllSoldiers();
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
            soldier.InvitedToCrowd(_bulletPool);
            _soldiers.Add(soldier);
            soldier.onDie.AddListener(RemoveFromCrowd);
        }

        private void RemoveFromCrowd(Soldier soldier)
        {
            _soldiers.Remove(soldier);
        }
    }
}