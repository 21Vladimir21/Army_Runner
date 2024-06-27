using System.Collections;
using _Main._Scripts.Obstacles;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.Soilders
{
    public class Bullet : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<GameObject> OnLifeTimeEnded;
        private float _lifeTime;
        private float _bulletSpeed;
        private bool _canMove;
        private int _damage;
        private Coroutine _deactivateRoutine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
            {
                if (obstacle.TryApplyDamage(_damage))
                {
                    DeactivateBullet();
                    StopCoroutine(_deactivateRoutine);
                }
            }
        }

        private void Update()
        {
            if (_canMove) MoveBullet();
        }

        public void Shot(float lifeTime, float speed, int damage, float bulletScale)
        {
            _lifeTime = lifeTime;
            _bulletSpeed = speed;
            _damage = damage;
            transform.localScale = Vector3.one * bulletScale;
            _deactivateRoutine = StartCoroutine(DeactivateRoutine());
            _canMove = true;
        }

        private IEnumerator DeactivateRoutine()
        {
            yield return new WaitForSeconds(_lifeTime);
            DeactivateBullet();
        }

        private void DeactivateBullet()
        {
            OnLifeTimeEnded.Invoke(gameObject);
            transform.localScale = Vector3.one;
            _canMove = false;
        }

        private void MoveBullet()
        {
            var forwardDirection = Vector3.forward;
            var moveDirection = forwardDirection.normalized * _bulletSpeed;
            transform.position += moveDirection * Time.deltaTime;
        }
    }
}