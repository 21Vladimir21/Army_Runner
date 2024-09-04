using LocalizationSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class PreGameView : AbstractView
    {
        [field: Header("Buttons")]
        
        [field: SerializeField] public Button AutoMergeSoldier { get; private set; }

        [field: SerializeField]
        public Button StartGameButton { get; private set; }

        [field: SerializeField] public Button RewardSoldier { get; private set; }
        [field: SerializeField] public Button SoundsButton { get; private set; }
        [field: SerializeField] public Button MusicButton { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }
        [field: SerializeField] public Button BackSettingsButton { get; private set; }
        [SerializeField] private Button hideRewardProposalButton;
        [field: SerializeField] public NoAdButton NoAdButton { get; private set; }

        [SerializeField] private GameObject settings;


        [field: Header("UpgradePanels")]
        [field: SerializeField]
        public UpgradePanelCell FireRateUpgradeCell { get; private set; }

        [field: SerializeField] public UpgradePanelCell DamageUpgradeCell { get; private set; }
        [field: SerializeField] public UpgradePanelCell BulletSpeedUpgradeCell { get; private set; }


        [Header("Texts")] [SerializeField] private TMP_Text moneyText;
        [field: SerializeField] public FormattableLocalizationTextTMP SoldierRewardText { get; private set; }
        [field: SerializeField] public FormattableLocalizationTextTMP LevelText { get; private set; }
        [Header("Sprites")] [SerializeField] private Sprite enableButtonSprite;
        [SerializeField] private Sprite disableButtonSprite;
        [SerializeField] private Sprite disableMusicButtonSprite;
        [SerializeField] private Sprite enableMusicButtonSprite;

        [SerializeField] private GameObject rewardProposal;


        private bool _isSoundButtonEnabled;
        private bool _isMusicButtonEnabled;
        private bool _isSettingActive;


        public override void Init()
        {
            base.Init();
            Saves.OnMoneyChanged += SetMoneyText;
            SetMoneyText();
            SoundsButton.onClick.AddListener(SwitchSoundButtonSprite);
            MusicButton.onClick.AddListener(SwitchMusicButtonSprite);
            _isSoundButtonEnabled = Saves.SoundEnabled;
            _isMusicButtonEnabled = Saves.MusicEnabled;
            SwitchSoundButtonSprite();
            SwitchMusicButtonSprite();
            NoAdButton.Init(Saves);
            hideRewardProposalButton.onClick.AddListener(HideRewardProposal);
            
            SettingsButton.onClick.AddListener(EnableSettings);
            BackSettingsButton.onClick.AddListener(EnableSettings);
        }

        public void ShowRewardProposal()
        {
            RewardSoldier.onClick.AddListener(HideRewardProposal);
            rewardProposal.SetActive(true);
        }

        private void HideRewardProposal()
        {
            RewardSoldier.onClick.RemoveListener(HideRewardProposal);
            rewardProposal.SetActive(false);
        }

        private void SetMoneyText() => moneyText.text = $"{Saves.Money}";

        //TODO:Говнокодим :)
        private void SwitchSoundButtonSprite()
        {
            if (_isSoundButtonEnabled)
                SoundsButton.image.sprite = enableButtonSprite;
            else
                SoundsButton.image.sprite = disableButtonSprite;
            _isSoundButtonEnabled = !_isSoundButtonEnabled;
        }

        private void SwitchMusicButtonSprite()
        {
            if (_isMusicButtonEnabled)
                MusicButton.image.sprite = enableMusicButtonSprite;
            else
                MusicButton.image.sprite = disableMusicButtonSprite;
            _isMusicButtonEnabled = !_isMusicButtonEnabled;
        }

        private void EnableSettings()
        {
            settings.SetActive(!_isSettingActive);
            _isSettingActive = !_isSettingActive;
        }
    }
}