using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace DadVSMe
{
    public struct StartGame
    {
        public async UniTask StartGameAsync(bool loadResources)
        {
            // FadeIn
            await DOFade.FadeInAsync();

            if(loadResources)
            {
                // Load Resources
                // await new LoadResourceByLabel().LoadAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);
            }

            // Load game scene
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_SCENE_NAME, LoadSceneMode.Single);
            GameInstance.GameCycle.InitializeAsync().Forget();

            // FadeOut
            await DOFade.FadeOutAsync(3.5f);
        }
    }
}