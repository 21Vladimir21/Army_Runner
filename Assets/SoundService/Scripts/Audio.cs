namespace SoundService.Scripts
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public static class Audio
    {
        public static void MuteAllAudio()
        {
            EventSystem.current.SetSelectedGameObject(null);
            AudioListener.pause = true;
            Time.timeScale = 0;
        }

        public static void UnMuteAllAudio(bool value = true)
        {
            AudioListener.pause = false;
            if (value) Time.timeScale = 1;
        }
    }
}