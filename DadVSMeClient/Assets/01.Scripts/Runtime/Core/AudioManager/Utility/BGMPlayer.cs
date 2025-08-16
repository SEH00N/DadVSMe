using UnityEngine;

namespace DadVSMe
{
    using static SourceVolumeFader;
    using static AudioManager;

    public class BGMPlayer : MonoBehaviour
    {
        private AudioSource player = null;
        public AudioSource Player => player;

        private SourceVolumeFader volumeFader = null;
        private BGMAudioLibraryTable currentBGMTable = null;

        public EFadeState FadeState => volumeFader.FadeState;

        private void Awake()
        {
            player = GetComponent<AudioSource>();
            volumeFader = GetComponent<SourceVolumeFader>();
        }

        public void Initialize(BGMAudioLibraryTable bgmData)
        {
            currentBGMTable = bgmData;
        }

        public void PlayBGM(float progress = 0f)
        {
            AudioHelper.PlayAudio(player, currentBGMTable.audioClip, progress);
        }

        public void PauseBGM()
        {
            AudioHelper.PauseAudio(player);
        }

        public void ResumeBGM()
        {
            AudioHelper.ResumeAudio(player);
        }

        public void DoFadeIn()
        {
            if (currentBGMTable == null)
                return;

            ResumeBGM();
            volumeFader.DoFade(currentBGMTable.fadeInData, EFadeState.FadeIn, EFadeState.Playing);
        }

        public void DoFadeOut()
        {
            if (currentBGMTable == null)
            {
                PauseBGM();
                return;
            }

            volumeFader.DoFade(currentBGMTable.fadeOutData, EFadeState.FadeOut, EFadeState.None, PauseBGM);
        }

        public void DoReset()
        {
            volumeFader.DoReset();
        }
    }
}