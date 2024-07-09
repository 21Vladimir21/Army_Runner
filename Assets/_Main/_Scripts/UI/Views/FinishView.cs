using _Main._Scripts.UI.SkillCheckAD;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class FinishView : AbstractView
    {
        [field: SerializeField] public Button NoThanksButton { get; private set; }

        [field: SerializeField] public AdWheel ADWheel { get; private set; }
        [field: SerializeField] public GameObject WinPanel { get; private set; }
    }
}