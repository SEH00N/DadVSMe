using UnityEngine;
using UnityEngine.SceneManagement;

namespace DadVSMe
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] string ongoingSceneName = "Intro";

        private async void Start()
        {
            await GameManager.Instance.InitializeAsync();
            // await LocalizationSettings.InitializeAsync();

            SceneManager.LoadScene(ongoingSceneName);
        }
    }
}
