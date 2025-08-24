
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DadVSMe.UI
{
    public class GameOverUI : MonoBehaviour
    {
        public async void OnTouchRetryButton()
        {
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_SCENE_NAME, LoadSceneMode.Single);
            GameInstance.GameCycle.InitializeAsync().Forget();
        }

        public async void OnTouchTitleButton()
        {
            await new ReleaseResourceByLabel().ReleaseAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);
            _ = SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
