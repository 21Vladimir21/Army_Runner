using UnityEngine;

namespace _Main._Scripts.Obstacles
{
    public class PickUpMoney : MonoBehaviour
    {
        [field:Range(0,1000), SerializeField] public int Count { get; private set; }
        [SerializeField] private ParticleSystem particle;
        

        public void TakeMoney()
        {
            particle.Play();
            gameObject.SetActive(false);
        }
    }
}