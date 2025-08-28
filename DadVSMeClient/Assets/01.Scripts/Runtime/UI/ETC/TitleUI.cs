using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using DadVSMe.UI.Setting;
using H00N.Resources.Pools;

namespace DadVSMe.UI
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] AddressableAsset<SettingPopupUI> settingPopupUIPrefab = null;

        public async void OnTouchStartButton()
        {
            // FadeIn
            await DOFade.FadeInAsync();

            // Load Resources
            await new LoadResourceByLabel().LoadAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);

            // Load game scene
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_SCENE_NAME, LoadSceneMode.Single);
            GameInstance.GameCycle.InitializeAsync().Forget();

            // FadeOut
            await DOFade.FadeOutAsync();
        }
         
        public async void OnTouchSettingButton()
        {
            await settingPopupUIPrefab.InitializeAsync();

            var popup = PoolManager.Spawn<SettingPopupUI>(settingPopupUIPrefab, transform);
            popup.StretchRect();

            popup.Initialize().Forget();
        }
    }
}
