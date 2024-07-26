using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Main._Scripts.TutorialLogic.Steps
{
    public class ContinueButtonStep : TutorialStepBase
    {
        [SerializeField] private Button button;

        public override void Enter(Action showCallback = null)
        {
            button.onClick.AddListener(ExitCallback.Invoke);
            base.Enter(showCallback);
        }

        public override void Exit()
        {
            button.onClick.RemoveListener(ExitCallback.Invoke);
            base.Exit();
        }
    }
}