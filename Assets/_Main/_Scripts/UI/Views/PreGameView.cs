using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class PreGameView : AbstractView
    {
        [field: Header("Buttons")]
        [field: SerializeField]
        public Button StartGameButton { get; private set; }

        [field: SerializeField] public Button FireRateUpgradeButton { get; private set; }
        [field: SerializeField] public Button DamageUpgradeButton { get; private set; }
        [field: SerializeField] public Button BulletSpeedUpgradeButton { get; private set; }


        [Header("Texts")] [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text fireRateLevelText;
        [SerializeField] private TMP_Text bulletDamageLevelText;
        [SerializeField] private TMP_Text bulletSpeedLevelText;

        public override void Init()
        {
            base.Init();
            Saves.OnMoneyChanged += SetMoneyText;
            SetMoneyText();
            UpdateFireRateLevelText(Saves.FireRateLevel);
            UpdateBulletDamageLevelText(Saves.BulletDamageLevel);
            UpdateBulletSpeedLevelText(Saves.BulletSpeedLevel);
        }

        public void UpdateFireRateLevelText(int level) => fireRateLevelText.text = (level + 1).ToString();
        public void UpdateBulletDamageLevelText(int level) => bulletDamageLevelText.text = (level + 1).ToString();
        public void UpdateBulletSpeedLevelText(int level) => bulletSpeedLevelText.text = (level + 1).ToString();

        private void SetMoneyText() => moneyText.text = $"{Saves.Money}";
    }
}