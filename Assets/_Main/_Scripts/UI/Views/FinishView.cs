using System.Collections;
using _Main._Scripts.UI.SkillCheckAD;
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

        public  void Open()
        {
            base.Open();
            WinPanel.SetActive(false);
        }
        

        public void UpdateEnemyCountText(int count) => enemyCountText.text = count.ToString();

        public void ShowWinPanel() => StartCoroutine(ShowWithDelay(() => WinPanel.SetActive(true),1));
    }
}