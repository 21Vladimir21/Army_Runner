using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class GameOverView : AbstractView
    {
        [field: SerializeField] public Button BackButton { get; private set; }
        [field: SerializeField] public GameObject GameOverPanel{ get; private set; }
        private const float ShowGameOverPanelDelay = 3.5f;

        public void Open()
        {
            base.Open();
            StartCoroutine(ShowWithDelay(() => { GameOverPanel.SetActive(true); }, ShowGameOverPanelDelay));
        }

        public void Close()
        {
            base.Close();
            GameOverPanel.SetActive(false);
        }
        
    }
}