using System;
using _Main._Scripts.LevelsLogic.FinishLogic.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace _Main._Scripts.LevelsLogic.FinishLogic
{
    public class FinishDeathZone : MonoBehaviour
    {
        [HideInInspector]public UnityEvent OnEnemyTouchZone =  new();
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Enemy>()) 
                OnEnemyTouchZone.Invoke();
        }
    }
}