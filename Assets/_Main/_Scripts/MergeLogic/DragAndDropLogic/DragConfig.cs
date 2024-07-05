using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.MergeLogic.DragAndDropLogic
{
    [CreateAssetMenu(fileName = "DragConfig", menuName = "GameConfigs/DragAndDropConfigs", order = 0)]
    public class DragConfig : ScriptableObject
    {

        [Range(-100, 100)] public float maxXPosition;

        [Range(-100, 100)] public float minXPosition;

        [Range(-100, 100)] public float maxZPosition;

        [Range(-100, 100)] public float minZPosition;

        [Range(1, 100)] public float heightWhenLifting;
    }
}