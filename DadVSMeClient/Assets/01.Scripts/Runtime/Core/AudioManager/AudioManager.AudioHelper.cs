using UnityEngine;

namespace DadVSMe
{
    public partial class AudioManager
    {
        public static class AudioHelper
        {
            public static void PlayAudio(AudioSource audioSource, AudioClip audioClip, float progress = 0f)
            {
                if (audioClip == null)
                    return;

                if (audioSource.isPlaying)
                    audioSource.Pause();

                audioSource.clip = audioClip;
                audioSource.time = progress;
                audioSource.Play();
            }

            public static void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
            {
                if (audioClip == null)
                    return;

                audioSource.PlayOneShot(audioClip);
            }

            public static void PauseAudio(AudioSource audioSource)
            {
                audioSource.Pause();
            }
            
            public static void StopAudio(AudioSource audioSource)
            {
                audioSource.Stop();
            }

            public static void ResumeAudio(AudioSource audioSource)
            {
                audioSource.UnPause();
            }
        }
    }
}