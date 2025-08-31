
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DadVSMe.UI
{
    public class GameOverUI : MonoBehaviour
    {
        public async void OnTouchRetryButton()
        {
            // FadeIn
            await DOFade.FadeInAsync();

            // Load Game Scene
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_SCENE_NAME, LoadSceneMode.Single);

            // Initialize GameCycle
            await GameInstance.GameCycle.InitializeAsync();

            // FadeOut Immediately
            _ = DOFade.FadeOutAsync(3.5f);
        }

        public async void OnTouchTitleButton()
        {
            // FadeIn
            await DOFade.FadeInAsync();

            // Release Resources
            await new ReleaseResourceByLabel().ReleaseAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);

            // Load Title Scene
            await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);

            // FadeOut Immediately
            _ = DOFade.FadeOutAsync(0f);
        }
    }
}
