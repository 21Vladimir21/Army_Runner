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

        [field: SerializeField] public Button RewardSoldier { get; private set; }
        [field: SerializeField] public Button SoundsButton { get; private set; }

        [field: Header("UpgradePanels")]
        [field: SerializeField] public UpgradePanelCell FireRateUpgradeCell { get; private set; }
        [field: SerializeField] public UpgradePanelCell DamageUpgradeCell { get; private set; }
        [field: SerializeField] public UpgradePanelCell BulletSpeedUpgradeCell { get; private set; }


        [Header("Texts")]
        [SerializeField] private TMP_Text moneyText;
        [Header("Sprites")]
        [SerializeField] private Sprite enableButtonSprite;
        [SerializeField] private Sprite disableButtonSprite;
        
        private bool _isSoundButtonEnabled;


        public override void Init()
        {
            base.Init();
            Saves.OnMoneyChanged += SetMoneyText;
            SetMoneyText();
            SoundsButton.onClick.AddListener(SwitchSoundButtonSprite);
            _isSoundButtonEnabled = Saves.SoundEnabled;
            SwitchSoundButtonSprite();
            
        }


        private void SetMoneyText() => moneyText.text = $"{Saves.Money}";

        private void SwitchSoundButtonSprite()
        {
            if (_isSoundButtonEnabled)
                SoundsButton.image.sprite = enableButtonSprite;
            else
                SoundsButton.image.sprite = disableButtonSprite;
            _isSoundButtonEnabled = !_isSoundButtonEnabled;
        }
    }
}