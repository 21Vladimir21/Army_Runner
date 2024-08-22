using _Main._Scripts.SavesLogic;
using SoundService.Scripts;
using UnityEngine.UI;

public class SoundController
{
    private AudioService _audioService;
    private Saves _saves;
    private bool _soundEnabled;
    private bool _musicEnabled;


    public SoundController(Button soundButton, Button musicButton, AudioService audioService, Saves saves)
    {
        _saves = saves;
        _audioService = audioService;
        _soundEnabled = _saves.SoundEnabled;
        _musicEnabled = _saves.MusicEnabled;
        soundButton.onClick.AddListener(ToggleSound);
        musicButton.onClick.AddListener(ToggleMusic);
    }

    private void ToggleSound()
    {
        _soundEnabled = !_soundEnabled;
        _audioService.SetActiveSound(_soundEnabled);
        _saves.SoundEnabled = _soundEnabled;
    }

    private void ToggleMusic()
    {
        _musicEnabled = !_musicEnabled;
        _audioService.SetActiveMusic(_musicEnabled);
        _saves.MusicEnabled = _musicEnabled;
    }
}