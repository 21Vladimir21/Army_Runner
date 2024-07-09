using _Main._Scripts.LevelsLogic.FinishLogic;
using UnityEngine;

namespace _Main._Scripts.LevelsLogic
{
    public class Level : MonoBehaviour
    {
        [field:SerializeField] public Finish Finish { get; private set; }
        [field:SerializeField] public Transform PlayerSpawnPoint { get; private set; }
        
        
        
    }
}