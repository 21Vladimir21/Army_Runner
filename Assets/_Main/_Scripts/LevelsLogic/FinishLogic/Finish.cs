using System;
using System.Collections;
using System.Collections.Generic;
using _Main._Scripts.LevelsLogic.FinishLogic.Enemies;
using _Main._Scripts.PlayerLogic;
using _Main._Scripts.Soilders;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.LevelsLogic.FinishLogic
{
    public class Finish : MonoBehaviour
    {
        [field: SerializeField] public List<Enemy> Enemies { get; private set; }
        [field: SerializeField] public FinishDeathZone FinishDeathZone { get; private set; }
        [SerializeField] private Transform[] points;


        private const float MoveToPointDuration = 1f;
        public UnityEvent OnFinished { get; private set; } = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                OnFinished.Invoke();
        }

        public void StartEnemiesAttach()
        {
            foreach (var enemy in Enemies) enemy.StartMove();
        }

        public void StopEnemiesAttach()
        {
            foreach (var enemy in Enemies) enemy.StopMove();
        }

        public void SetSoldiersNewPosition(List<Soldier> soldiers,Action callback)
        {
            StartCoroutine(SetPositionCallbackRoutine(callback));
            for (var i = 0; i < soldiers.Count; i++)
                soldiers[i].transform.DOMove(points[i].position, MoveToPointDuration);
            
        }

        private IEnumerator SetPositionCallbackRoutine(Action callback)
        {
            yield return new WaitForSeconds(MoveToPointDuration);
            callback?.Invoke();
        }
    }
}