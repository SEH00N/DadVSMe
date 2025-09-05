using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using H00N.Resources.Addressables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DadVSMe.UI
{
    public class IntroButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float DEFAULT_SCALE = 1f;
        private const float HOVER_SCALE = 1.1f;
        private const float HOVER_DURATION = 0.2f;

        private const float ANIMATION_DURATION = 0.6f;

        [SerializeField] AudioClip clickSound = null;
        [SerializeField] AudioClip tweenSound = null;
        [SerializeField] CanvasGroup canvasGroup = null;
        [SerializeField] Ease enterEase = Ease.OutBack;
        [SerializeField] Ease exitEase = Ease.OutBack;

        private bool isTweening = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(isTweening)
                return;

            transform.DOKill();

            float scale = transform.localScale.x;
            float delta = (HOVER_SCALE - scale) / (HOVER_SCALE - DEFAULT_SCALE);
            if(delta <= 0)
            {
                transform.localScale = Vector3.one * HOVER_SCALE;
                return;
            }

            transform.DOScale(HOVER_SCALE, HOVER_DURATION * delta).SetEase(enterEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(isTweening)
                return;

            transform.DOKill();

            float scale = transform.localScale.x;
            float delta = (DEFAULT_SCALE - scale) / (DEFAULT_SCALE - HOVER_SCALE);
            if(delta <= 0)
            {
                transform.localScale = Vector3.one * DEFAULT_SCALE;
                return;
            }

            transform.DOScale(DEFAULT_SCALE, HOVER_DURATION * delta).SetEase(exitEase);
        }

        public async void OnTouchThis()
        {
            if(isTweening)
                return;

            isTweening = true;
            transform.DOKill();

            AudioManager.Instance.PlaySFX(clickSound, force: true);

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = false;
            transform.localScale = Vector3.one * DEFAULT_SCALE;

            await transform.DOLocalRotate(new Vector3(0, 0, 360), ANIMATION_DURATION, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart);

            AudioManager.Instance.PlaySFX(tweenSound, force: true);

            _ = canvasGroup.DOFade(0, ANIMATION_DURATION).SetEase(Ease.Linear);
            _ = transform.DOLocalRotate(new Vector3(0, 0, 360 * 2.5f), ANIMATION_DURATION, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            _ = transform.DOScale(Vector3.zero, ANIMATION_DURATION).SetEase(Ease.Linear);

            await UniTask.Delay(TimeSpan.FromSeconds(ANIMATION_DURATION));

            isTweening = false;
            transform.parent.gameObject.SetActive(false);
        }
    }
}