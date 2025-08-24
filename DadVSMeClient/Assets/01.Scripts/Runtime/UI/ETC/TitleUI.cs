using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace DadVSMe.UI
{
    public class TitleUI : MonoBehaviour
    {

        public async void OnTouchStartButton()
        {
            // FadeIn
            await DOFade.FadeInAsync();

            // Load Resources
            await Addressables.InstantiateAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);

            // Load game scene
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_SCENE_NAME, LoadSceneMode.Single);

            // FadeOut
            await DOFade.FadeOutAsync();
        }
    }
}
