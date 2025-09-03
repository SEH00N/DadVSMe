using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using DadVSMe.UI.Setting;
using H00N.Resources.Pools;
using System.Collections.Generic;

namespace DadVSMe.UI
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] AddressableAsset<SettingPopupUI> settingPopupUIPrefab = null;
        [SerializeField] List<AddressableAsset<AudioClip>> transitionSounds = null;
        [SerializeField] AddressableAsset<AudioClip> clickSound = null;

        private void Awake()
        {
            transitionSounds.ForEach(sound => sound.InitializeAsync().Forget());
            clickSound.InitializeAsync().Forget();
        }

        public async void OnTouchStartButton()
        {
            _ = new PlaySound(clickSound);

            // FadeIn
            await DOFade.FadeInAsync();

            // Load Resources
            await new LoadResourceByLabel().LoadAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);

            // Load game scene
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_SCENE_NAME, LoadSceneMode.Single);
            GameInstance.GameCycle.InitializeAsync().Forget();

            // FadeOut
            await DOFade.FadeOutAsync(3.5f);
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
            _ = new PlaySound(clickSound);
        }

        public void PlaySound(int index)
        {
            _ = new PlaySound(transitionSounds[index]);
        }
    }
}
