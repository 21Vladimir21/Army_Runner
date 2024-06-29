using UnityEngine;

namespace _Main._Scripts.Boosts
{
    public class Boost : MonoBehaviour
    {
        [field: SerializeField] public BoostType Type { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}