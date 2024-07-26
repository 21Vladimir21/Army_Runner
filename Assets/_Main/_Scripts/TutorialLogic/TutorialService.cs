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
            if (_saves.TutorialStepIndex >=8) 
                _nextStepIndex = _saves.TutorialStepIndex;
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

        public void HideCurrentStep() => _currentStep.Hide();

        public void ResetTutorial()
        {
            if (_saves.TutorialStepIndex <= 7)
            {
                _currentStep.Exit();
                _saves.TutorialStepIndex = 0;
                _nextStepIndex = 0;
            } 
        }
    }
}