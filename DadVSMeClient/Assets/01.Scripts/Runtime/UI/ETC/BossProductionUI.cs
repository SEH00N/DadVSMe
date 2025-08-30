using Cysharp.Threading.Tasks;
using DadVSMe.UI;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe
{
    public class BossProductionUI : PoolableBehaviourUI
    {
        [Header("Transform")]
        [SerializeField] RectTransform _bannerTransform;
        [SerializeField] RectTransform _profileTransform;

        [Header("Banner")]
        [SerializeField] Vector2 _bannerStartPosition;
        [SerializeField] Vector2 _bannerNormalPosition;
        [SerializeField] Vector2 _bannerEndPosition;

        [Header("Profile")]
        [SerializeField] Image _profileImage;
        [SerializeField] Vector2 _profileStartPosition;
        [SerializeField] Vector2 _profileNormalPosition;
        [SerializeField] Vector2 _profileIntervalPosition;
        [SerializeField] Vector2 _profileEndPosition;

        [Header("Background")]
        [SerializeField] CanvasGroup _backgroundImageCanvasGroup;

        private const float APPEAR_TIME = 0.6f;
        private const float WAIT_TIME = 0.8f;
        private const float INTERVAL_TIME = 0.2f;
        private const float DISAPPEAR_TIME = 0.5f;
        private const float BACKGROUND_TRANSITION_TIME_RATIO = 0.3f;

        public async UniTask Initialize(Sprite bossVisual)
        {
            base.Initialize();

            _profileImage.sprite = bossVisual;
            _bannerTransform.anchoredPosition = _bannerStartPosition;
            _profileTransform.anchoredPosition = _profileStartPosition;
            _backgroundImageCanvasGroup.alpha = 0f;

            await UniTask.DelayFrame(3);

            _ = _backgroundImageCanvasGroup.DOFade(1f, APPEAR_TIME * BACKGROUND_TRANSITION_TIME_RATIO).SetEase(Ease.InQuart).SetUpdate(true);

            _ = _bannerTransform.DOLocalMove(_bannerNormalPosition, APPEAR_TIME).SetEase(Ease.InQuart).SetUpdate(true);
            await UniTask.Delay(TimeSpan.FromSeconds(INTERVAL_TIME), true);
            _ = _profileTransform.DOLocalMove(_profileNormalPosition, APPEAR_TIME).SetEase(Ease.InQuart).SetUpdate(true);
            await UniTask.Delay(TimeSpan.FromSeconds(APPEAR_TIME), true);
            await _profileTransform.DOLocalMove(_profileIntervalPosition, WAIT_TIME).SetUpdate(true);

            _ = _profileTransform.DOLocalMove(_profileEndPosition, DISAPPEAR_TIME).SetEase(Ease.InExpo).SetUpdate(true);
            await UniTask.Delay(TimeSpan.FromSeconds(INTERVAL_TIME), true);
            await _bannerTransform.DOLocalMove(_bannerEndPosition, DISAPPEAR_TIME).SetEase(Ease.InExpo).SetUpdate(true);

            await _backgroundImageCanvasGroup.DOFade(0f, APPEAR_TIME * BACKGROUND_TRANSITION_TIME_RATIO).SetEase(Ease.InQuart).SetUpdate(true);
        }
    }
}
