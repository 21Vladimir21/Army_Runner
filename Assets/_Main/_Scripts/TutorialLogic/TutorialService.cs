using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services;
using UnityEngine;

namespace _Main._Scripts.TutorialLogic
{
    public class TutorialService : MonoBehaviour, IService
    {
        [SerializeField] private TutorialStepBase[] tutorialSteps;
        private TutorialStepBase _currentStep;
        private int _nextStepIndex;
        private Saves _saves;

        public void Init(Saves saves)
        {
            _saves = saves;
            foreach (var step in tutorialSteps)
                step.ExitCallback += TryCallNextStep;
            if (_saves.TutorialStepIndex >= 7) 
                _nextStepIndex = _saves.TutorialStepIndex;
            TryCallNextStep();
        }

        public void TryCallNextStep()
        {
            if (_saves.WasShowedTutorial) return;

            _saves.TutorialStepIndex = _nextStepIndex;
            if (_currentStep != null) _currentStep.Exit();
            if (_nextStepIndex >= tutorialSteps.Length)
            {
                _saves.WasShowedTutorial = true;
                return;
            }

            _currentStep = tutorialSteps[_nextStepIndex];
            _currentStep.Enter();
            _nextStepIndex++;
        }
    }
}