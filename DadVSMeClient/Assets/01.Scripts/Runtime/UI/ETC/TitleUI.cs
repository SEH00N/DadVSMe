using UnityEngine;
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
            _ = new PlaySound(clickSound);
        }

        public void PlaySound(int index)
        {
            _ = new PlaySound(transitionSounds[index]);
        }
    }
}
