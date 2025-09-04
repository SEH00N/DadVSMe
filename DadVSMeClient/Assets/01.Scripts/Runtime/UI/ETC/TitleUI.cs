using UnityEngine;
using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using DadVSMe.UI.Setting;
using H00N.Resources.Pools;
using System.Collections.Generic;
using DG.Tweening;

namespace DadVSMe.UI
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] AddressableAsset<SettingPopupUI> settingPopupUIPrefab = null;
        [SerializeField] List<AddressableAsset<AudioClip>> transitionSounds = null;
        [SerializeField] AddressableAsset<AudioClip> clickSound = null;

        [Space(10f)]
        [SerializeField] GameObject howToPlayPanelObject = null;
        [SerializeField] GameObject howToPlayContentsObject = null;
        [SerializeField] CanvasGroup howToPlayPanelCanvasGroup = null;

        private bool isTweening = false;

        private void Awake()
        {
            transitionSounds.ForEach(sound => sound.InitializeAsync().Forget());
            clickSound.InitializeAsync().Forget();
        }

        public void OnTouchStartButton()
        {
            _ = new PlaySound(clickSound);
            _ = new StartGame().StartGameAsync(loadResources: true);
        }
         
        public async void OnTouchSettingButton()
        {
            await settingPopupUIPrefab.InitializeAsync();

            var popup = PoolManager.Spawn<SettingPopupUI>(settingPopupUIPrefab, transform);
            popup.StretchRect();
            // _ = new PlaySound(clickSound);

            popup.Initialize().Forget();
        }

        public void OnTouchHowToPlayButton()
        {
            if(isTweening)
                return;

            isTweening = true;

            _ = new PlaySound(clickSound);
            howToPlayPanelObject.SetActive(true);
            howToPlayContentsObject.transform.localScale = Vector3.one;
            howToPlayPanelCanvasGroup.alpha = 0f;
            howToPlayPanelCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => {
                isTweening = false;
            });
        }

        public void OnTouchCloseHowToPlayButton()
        {
            if(isTweening)
                return;

            isTweening = true;

            howToPlayContentsObject.transform.DOScale(0f, 0.2f).SetEase(Ease.OutCubic).OnComplete(() => {
                howToPlayPanelObject.SetActive(false);
                isTweening = false;
            });
        }

        public void PlaySound(int index)
        {
            _ = new PlaySound(transitionSounds[index]);
        }
    }
}
