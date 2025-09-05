using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DadVSMe
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] string ongoingSceneName = "IntroScene";

        private async void Start()
        {
            await GameManager.Instance.InitializeAsync();
            // await LocalizationSettings.InitializeAsync();

            // await new LoadResourceByLabel().LoadAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);
            await new LoadResourceByLabel().LoadAsync(GameDefine.ADDRESSABLES_LABEL_PRELOAD);

            await SceneManager.TryLoadSceneAsync(ongoingSceneName, LoadSceneMode.Single);
        }
    }
}
