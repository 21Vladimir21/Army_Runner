using System.Collections;
using _Main._Scripts.UI.SkillCheckAD;
using LocalizationSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class FinishView : AbstractView
    {
        [field: SerializeField] public Button NoThanksButton { get; private set; }
        [field: SerializeField] public AdWheel ADWheel { get; private set; }
        [field: SerializeField] public GameObject WinPanel { get; private set; }
        [SerializeField] private TMP_Text enemyCountText;
        [SerializeField] private FormattableLocalizationTextTMP MoneyCollected;
        [SerializeField] private FormattableLocalizationTextTMP SoldiersCollected;
        [SerializeField] private RandomGeneralPhrase phrases;
    

        private const float ShowWinPanelDelay = 3f;


        public  void Open()
        {
            base.Open();
            WinPanel.SetActive(false);
        }
        

        public void UpdateEnemyCountText(int count) => enemyCountText.text = count.ToString();

        public void ShowWinPanel(int moneyCount,int soldiersCount)
        {
            MoneyCollected.SetValue(moneyCount);
            SoldiersCollected.SetValue(soldiersCount);
            StartCoroutine(ShowWithDelay(() =>
            {
                WinPanel.SetActive(true);
                phrases.ShowRandomPhrase();
            }, ShowWinPanelDelay));
        }
    }
}