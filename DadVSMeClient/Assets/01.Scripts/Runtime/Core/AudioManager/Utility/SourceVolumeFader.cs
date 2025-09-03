using System;
using System.Collections;
using UnityEngine;

namespace DadVSMe
{
    public partial class SourceVolumeFader : MonoBehaviour
    {
        private EFadeState fadeState = EFadeState.None;
        public EFadeState FadeState => fadeState;

        private AudioSource player = null;
        private float process = 0f;

        private void Awake()
        {
            player = GetComponent<AudioSource>();
        }

        #if UNITY_EDITOR
        [SerializeField, Range(0f, 1f)] float slider = 0f;
        private void Update()
        {
            if (player.clip)
                slider = player.time / player.clip.length;
        }
        #endif

        public void DoFade(FadeData fadeData, EFadeState fadeState, EFadeState targetFadeState, Action callback = null)
        {
            if (this.fadeState != fadeState)
                process = 0f;

            this.fadeState = fadeState;
            StopAllCoroutines();

            float volumeLimit = fadeState == EFadeState.FadeOut ? player.volume : 1f;
            StartCoroutine(FadeRoutine(fadeData.fadeDuration, fadeData.fadeCurve, volumeLimit, targetFadeState, callback));
        }

        public void DoReset()
        {
            StopAllCoroutines();
            player.volume = 1f;
            process = 0f;
            fadeState = EFadeState.None;
        }

        private IEnumerator FadeRoutine(float fadeDuration, AnimationCurve fadeCurve, float volumeLimit, EFadeState targetFadeState, Action callback)
        {
            if (fadeDuration <= 0f)
            {
                player.volume = 1f;
                fadeState = targetFadeState;
                callback?.Invoke();
                yield break;
            }

            float duration = Mathf.Min(fadeDuration, player.clip.length);
            float timer = Mathf.Lerp(0, duration, process);
            while (timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                process = timer / duration;

                float volume = fadeCurve.Evaluate(process);
                volume = Mathf.Clamp(volume, 0, volumeLimit);
                player.volume = volume;

                yield return null;
            }

            player.volume = fadeCurve.Evaluate(1f);
            fadeState = targetFadeState;

            callback?.Invoke();
        }
    }
}