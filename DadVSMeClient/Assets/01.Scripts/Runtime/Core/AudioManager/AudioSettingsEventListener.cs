using UnityEngine;

namespace DadVSMe
{
    public class AudioSettingsEventListener : MonoBehaviour
    {
        private void Awake()
        {
            AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
            AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
        }

        private void OnDestroy()
        {
            AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
        }

        private void OnAudioConfigurationChanged(bool deviceWasChanged)
        {
            if (AudioManager.Instance == null)
                return;

            AudioManager.Instance.InitializeVolume();
            AudioManager.Instance.PauseBGM(true);
            AudioManager.Instance.ResumeBGM(true);
        }
    }
}