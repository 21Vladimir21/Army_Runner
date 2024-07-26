using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class GameOverView : AbstractView
    {
        [field: SerializeField] public Button BackButton { get; private set; }
        [field: SerializeField] public GameObject GameOverPanel{ get; private set; }
        [SerializeField] private RandomGeneralPhrase phrases;
        private const float ShowGameOverPanelDelay = 2.5f;

        public void Open()
        {
            base.Open();
            
            StartCoroutine(ShowWithDelay(() =>
            {
                GameOverPanel.SetActive(true);
                phrases.ShowRandomPhrase();
            }, ShowGameOverPanelDelay));
        }

        public void Close()
        {
            base.Close();
            GameOverPanel.SetActive(false);
        }
        
    }
}