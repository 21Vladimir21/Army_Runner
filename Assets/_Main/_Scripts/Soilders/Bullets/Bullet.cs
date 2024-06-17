using System;
using System.Collections;
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
        private Coroutine _deactivateRoutine;


        private void Update()
        {
            if (_canMove) MoveBullet();
        }

        public void Shot(float lifeTime, float bulletSpeed)
        {
            _lifeTime = lifeTime;
            _bulletSpeed = bulletSpeed;
            _deactivateRoutine = StartCoroutine(DeactivateRoutine());
            _canMove = true;
        }

        private IEnumerator DeactivateRoutine()
        {
            yield return new WaitForSeconds(_lifeTime);
            OnLifeTimeEnded.Invoke(gameObject);
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