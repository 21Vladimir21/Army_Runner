using UnityEngine;

namespace _Main._Scripts.Boosts
{
    public class DamageBoost : MonoBehaviour
    {
        [field:SerializeField] public float DamageRatio { get; private set; }

        public void Take()
        {
        }
    }
}