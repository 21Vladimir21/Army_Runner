using System.Collections;
using _Main._Scripts.Soilders.Bullets;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.Soilders
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        [HideInInspector] public UnityEvent<GameObject> OnLifeTimeEnded;
        private float _lifeTime;
        private float _bulletSpeed;
        private bool _canMove;
        private float _damage;
        private Coroutine _deactivateRoutine;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.TryApplyDamage(_damage))
                {
                    StartCoroutine(DeactivateBullet());
                    StopCoroutine(_deactivateRoutine);
                }
            }
        }

        private void Update()
        {
            if (_canMove) MoveBullet();
        }

        public void Shot(float lifeTime, float speed, float damage, float bulletScale)
        {
            _lifeTime = lifeTime;
            _bulletSpeed = speed;
            _damage = damage;
            transform.localScale = Vector3.one *  transform.localScale.x / 100 * bulletScale;
            _deactivateRoutine = StartCoroutine(DeactivateRoutine());
            _canMove = true;
        }

        private IEnumerator DeactivateRoutine()
        {
            yield return new WaitForSeconds(_lifeTime);
            yield return DeactivateBullet();
        }

        private IEnumerator DeactivateBullet()
        {
            particle.Play();
            yield return new WaitForSeconds(0.1f);
            OnLifeTimeEnded.Invoke(gameObject);
            transform.localScale = Vector3.one;
            _canMove = false;
        }

        private void MoveBullet()
        {
            var forwardDirection = transform.forward;
            var moveDirection = forwardDirection.normalized * _bulletSpeed;
            transform.position += moveDirection * Time.deltaTime;
        }
    }
}