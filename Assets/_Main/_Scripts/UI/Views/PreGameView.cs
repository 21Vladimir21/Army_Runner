using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class PreGameView : AbstractView
    {
        [field: SerializeField] public Button StartGameButton { get; private set; }
        
        [field: SerializeField] public Button FireRateUpgradeButton { get; private set; }
        [field: SerializeField] public Button DamageUpgradeButton { get; private set; }
        [field: SerializeField] public Button BulletSpeedUpgradeButton { get; private set; }
        
        [SerializeField] private TMP_Text moneyText;

        public override void Init()
        {
            base.Init();
            Saves.OnMoneyChanged += SetMoneyText;
            SetMoneyText();
        }

        private void SetMoneyText() => moneyText.text = $"{Saves.Money}";
    }
}