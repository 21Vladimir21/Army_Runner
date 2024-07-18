using _Main._Scripts.UI.Anminations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class UpgradePanelCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text costText;
        [field: SerializeField] public TMP_Text LevelText { get; private set; }
        [field: SerializeField] public Button BuyButton { get; private set; }
        [SerializeField] private Image icon;
        [SerializeField] private Sprite maxlevelIcon;
        [field:SerializeField] public PercentageFadeTextAnimation TextAnimation { get; private set; }


        public void SetMaxLevel()
        {
            icon.sprite = maxlevelIcon;
            BuyButton.interactable = false;
            costText.text = "max";
        }

        public void UpdateCellTexts(int level, int cost, float percentage)
        {
            TextAnimation.StartAnimation(percentage);
            LevelText.text = (level + 1).ToString();
            costText.text = cost.ToString();
        }
    }
}