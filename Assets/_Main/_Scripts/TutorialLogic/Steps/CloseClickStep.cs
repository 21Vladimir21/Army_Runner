using System;
using UnityEngine.EventSystems;

namespace _Main._Scripts.TutorialLogic.Steps
{
    public class CloseClickStep: TutorialStepBase, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData) => gameObject.SetActive(false);
        public override void Enter(Action showCallback = null)
        {
            base.Enter(showCallback);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}