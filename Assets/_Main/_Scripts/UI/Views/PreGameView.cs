using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.UI
{
    public class PreGameView : AbstractView
    {
        [field: SerializeField] public Button StartGameButton { get; private set; }
    }
}