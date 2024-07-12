using UnityEngine;

namespace _Main._Scripts.Obstacles
{
    public class PickUpMoney :  DescendingObject
    {
        [field:Range(0,1000), SerializeField] public int Count { get; private set; }
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject moneyModel;
        

        public void TakeMoney()
        {
            DeactivateTrigger();
            particle.Play();
            moneyModel.SetActive(false);
        }

    }
}