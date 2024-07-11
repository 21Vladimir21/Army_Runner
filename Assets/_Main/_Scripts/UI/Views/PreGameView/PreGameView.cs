using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class PreGameView : AbstractView
    {
        [field: Header("Buttons")]
        [field: SerializeField] public Button StartGameButton { get; private set; }
        [field: SerializeField] public Button RewardSoldier { get; private set; }

        [field: SerializeField] public UpgradePanelCell FireRateUpgradeCell { get; private set; }
        [field: SerializeField] public UpgradePanelCell DamageUpgradeCell { get; private set; }
        [field: SerializeField] public UpgradePanelCell BulletSpeedUpgradeCell { get; private set; }


        [Header("Texts")] [SerializeField] private TMP_Text moneyText;


        public override void Init()
        {
            base.Init();
            Saves.OnMoneyChanged += SetMoneyText;
            SetMoneyText();
        }
        

        private void SetMoneyText() => moneyText.text = $"{Saves.Money}";
    }
}