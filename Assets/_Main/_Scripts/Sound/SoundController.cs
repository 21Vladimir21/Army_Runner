using _Main._Scripts.SavesLogic;
using SoundService.Scripts;
using UnityEngine.UI;

public class SoundController
{
    private AudioService _audioService;
    private Saves _saves;
    private bool _soundEnabled;
    

    public SoundController(Button button, AudioService audioService, Saves saves)
    {
        _saves = saves;
        _audioService = audioService;
        _soundEnabled = _saves.SoundEnabled;
        button.onClick.AddListener(ToggleSound);
    }


    private void ToggleSound()
    {
        _soundEnabled = !_soundEnabled;
        _audioService.SetActiveSound(_soundEnabled);
        _saves.SoundEnabled = _soundEnabled;
    }
}
