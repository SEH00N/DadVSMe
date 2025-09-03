
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DadVSMe.UI
{
    public class GameOverUI : MonoBehaviour
    {
        public void OnTouchRetryButton()
        {
            _ = new StartGame().StartGameAsync(loadResources: false);
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
