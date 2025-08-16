using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DadVSMe
{
    public partial class AudioManager : AudioManagerBase
    {
        public static AudioManager Instance { get; private set; }

        private const float PLAY_INTERVAL_THRESHOLD = 0.1f;

        [SerializeField] AudioMixer audioMixer = null;
        [SerializeField] BGMController bgmController = null;
        [SerializeField] AudioSource sfxPlayer = null; // non-pauseable

        private Dictionary<string, float> effectAudioPlayTimeBuffer = null;

        public void Initialize()
        {
            Instance = this;

            effectAudioPlayTimeBuffer = new Dictionary<string, float>();
            base.Initialize(audioMixer);
            InitializeVolume();

            AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
            AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
        }

        public void Release()
        {
            Instance = null;

            effectAudioPlayTimeBuffer.Clear();

            PauseBGM(true);
            bgmController.ClearCache();
        }

        public void InitializeVolume()
        {
            SetVolume(EAudioChannel.Master, GameSettings.MasterVolume);
            SetVolume(EAudioChannel.BGM, GameSettings.BGMVolume);
            SetVolume(EAudioChannel.SFX, GameSettings.SFXVolume);
        }

        private void OnAudioConfigurationChanged(bool deviceWasChanged)
        {
            InitializeVolume();
            PauseBGM(true);
            ResumeBGM(true);
        }

        public void PlayBGM(BGMAudioLibrary bgmLibrary, bool immediately = false, bool loadCache = true) 
        {
            bgmController.PlayBGM(bgmLibrary, immediately, loadCache);
        }
        
        public void PauseBGM(bool immediately = false) 
        {
            bgmController.PauseBGM(immediately);
        }
        
        public void ResumeBGM(bool immediately = false) 
        {
            bgmController.ResumeBGM(immediately);
        }

        public void PlaySFX(AudioClip clip) => PlaySFX(clip.name, clip);
        private void PlaySFX(string audioName, AudioClip clip)
        {

            if (effectAudioPlayTimeBuffer.ContainsKey(audioName) == false)
                effectAudioPlayTimeBuffer.Add(audioName, float.MinValue);

            if (Time.time - effectAudioPlayTimeBuffer[audioName] <= PLAY_INTERVAL_THRESHOLD)
                return;

            AudioHelper.PlayOneShot(sfxPlayer, clip);
            effectAudioPlayTimeBuffer[audioName] = Time.time;
        }
    }
}