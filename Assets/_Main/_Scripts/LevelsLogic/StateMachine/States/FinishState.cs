using _Main._Scripts.PlayerLogic.StateMachine;
using _Main._Scripts.PlayerLogic.StateMachine.States;
using _Main._Scripts.SavesLogic;
using _Main._Scripts.Services.Cameras;

namespace _Main._Scripts.Level.StateMachine.States
{
    public class FinishState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly CameraService _cameraService;
        private readonly Saves _saves;

        public FinishState(IStateSwitcher stateSwitcher, CameraService cameraService, Saves saves)
        {
            _stateSwitcher = stateSwitcher;
            _cameraService = cameraService;
            _saves = saves;
        }

        public void Enter()
        {
            _cameraService.SwitchToFromType(CameraType.FinishCamera);
            
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}