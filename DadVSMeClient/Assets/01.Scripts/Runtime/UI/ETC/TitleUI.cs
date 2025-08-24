using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace DadVSMe.UI
{
    public class TitleUI : MonoBehaviour
    {

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
    }
}
