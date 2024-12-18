using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

namespace _Main._Scripts.UI
{
    public class GameView : AbstractView
    {
        [field: SerializeField] public Button RestartButton { get; private set; }
        [Space] [SerializeField] private Slider levelProgressBar;
        [Space] [SerializeField] private TMP_Text damageStat;
        [SerializeField] private TMP_Text bulletSpeedStat;
        [SerializeField] private TMP_Text fireRateStat;

        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text nextLevelText;
        [SerializeField] private TMP_Text soldiersCountText;


        public void SetLevelText(int level)
        {
            currentLevelText.text = (level + 1).ToString();
            nextLevelText.text = (level + 2).ToString();
        }

        public void UpdateProgressBar(float value) => levelProgressBar.value = Mathf.Clamp01(value);
        public void UpdateSoldiersCountText(int count) => soldiersCountText.text = count.ToString();

        public void UpdateStats(float damage, float fireRate, float bulletSpeed)
        {
            damageStat.text = damage + "%";
            bulletSpeedStat.text = bulletSpeed + "%";
            fireRateStat.text = 100 + (100 - Mathf.Abs(fireRate)) + "%";
        }
    }
}