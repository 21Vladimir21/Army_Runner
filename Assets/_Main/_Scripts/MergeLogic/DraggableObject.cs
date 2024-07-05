using _Main._Scripts.CrowdLogic;
using UnityEngine;

namespace _Main._Scripts.MergeLogic
{
    public class DraggableObject : MonoBehaviour,ISoldier
    {
        [field: SerializeField] public SoldiersLevels Level { get; private set; }
        
        
        //TODO:События подбора и отпускания , смена анимации и подобное 
    }
}