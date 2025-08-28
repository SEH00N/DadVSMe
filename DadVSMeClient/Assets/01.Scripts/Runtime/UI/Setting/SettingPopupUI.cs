using UnityEngine;
using H00N.Resources.Pools;
using DadVSMe.UI;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;

namespace DadVSMe.UI.Setting
{
    public class SettingPopupUI : PoolableBehaviourUI
    {
        private const string BLOCK_KEY = "SettingPopupUI";

        [Header("Slider")]
        [SerializeField] Slider _masterVolumeSlider;
        [SerializeField] Slider _bgmVolumeSlider;
        [SerializeField] Slider _sfxVolumeSlider;

        [Header("Animation")]
        [SerializeField] Image _backgroundDimmed;
        [SerializeField] Transform _panelTransform;

        private const float APPEAR_TIME = 0.3f;
        private const float DISAPPEAR_TIME = 0.2f;

        private const float DIMMED_VALUE = 0.5f;

        public async UniTask Initialize()
        {
            _masterVolumeSlider.value = GetNormalizedVolume(EAudioChannel.Master);
            _bgmVolumeSlider.value = GetNormalizedVolume(EAudioChannel.BGM);
            _sfxVolumeSlider.value = GetNormalizedVolume(EAudioChannel.SFX);

            InputBlock.Block(BLOCK_KEY);
            await PlayAppearAnimation();
            InputBlock.Release(BLOCK_KEY);
        }

        private float GetNormalizedVolume(EAudioChannel channel)
        {
            float dbValue = AudioManager.Instance.GetVolume(channel);

            if(dbValue <= AudioManager.AUDIO_MIN_VOLUME)
                return 0f;

            float linearVolume = Mathf.Pow(10f, dbValue / 20f);

            if (AudioManager.Instance.IsBassMode(channel))
                linearVolume *= 2f;

            return Mathf.Clamp01(linearVolume);
        }

        private async UniTask PlayAppearAnimation()
        {
            Color color = _backgroundDimmed.color;
            color.a = 0;
            _backgroundDimmed.color = color;

            _panelTransform.localScale = Vector2.zero;

            await UniTask.DelayFrame(1);

            _ = _backgroundDimmed.DOFade(DIMMED_VALUE, APPEAR_TIME);
            _ = _panelTransform.DOScale(Vector2.one, APPEAR_TIME).SetEase(Ease.OutBack);

            await UniTask.Delay(TimeSpan.FromSeconds(APPEAR_TIME));
        }

        public void OnValueChangedMasterValue(float value)
        {
            AudioManager.Instance.SetVolume(EAudioChannel.Master, value);
        }

        public void OnValueeChangedBGMValue(float value)
        {
            AudioManager.Instance.SetVolume(EAudioChannel.BGM, value);
        }

        public void OnValueChangedSFXValue(float value)
        {
            AudioManager.Instance.SetVolume(EAudioChannel.SFX, value);
        }

        public async void OnTouchConfirmButton()
        {
            InputBlock.Block(BLOCK_KEY);
            await PlayDisappearAnimation();
            InputBlock.Release(BLOCK_KEY);

            PoolManager.Despawn(this);
        }

        private async UniTask PlayDisappearAnimation()
        {
            Color color = _backgroundDimmed.color;
            color.a = DIMMED_VALUE;
            _backgroundDimmed.color = color;

            _panelTransform.localScale = Vector2.one;

            await UniTask.DelayFrame(1);

            _ = _backgroundDimmed.DOFade(0, DISAPPEAR_TIME);
            _ = _panelTransform.DOScale(Vector2.zero, DISAPPEAR_TIME).SetEase(Ease.InBack);

            await UniTask.Delay(TimeSpan.FromSeconds(DISAPPEAR_TIME));
        }
    }
}
