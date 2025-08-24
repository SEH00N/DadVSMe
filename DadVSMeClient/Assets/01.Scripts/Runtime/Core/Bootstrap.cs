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

            await SceneManager.TryLoadSceneAsync(ongoingSceneName, LoadSceneMode.Single);
        }
    }
}
