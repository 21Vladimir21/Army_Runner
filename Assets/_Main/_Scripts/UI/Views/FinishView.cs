using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class FinishView : AbstractView
    {
        [field: SerializeField] public Button NextLevelButton { get; private set; }
    }
}