using DadVSMe.Inputs;
using H00N.Resources;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace DadVSMe
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null)
            {
                Instance.Release();
                DestroyImmediate(Instance.gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public UniTask InitializeAsync()
        {
            // Core Initialize
            ResourceManager.Initialize(new AddressableResourceLoader());
            PoolManager.Initialize(transform);
            SceneManager.Initialize();
            InputManager.Initialize();
            TimeManager.Initialize();

            // Game Manager Initialize
            gameObject.GetComponent<AudioManager>().Initialize();

            // Game Initialize
            Application.targetFrameRate = 60;
            InputManager.EnableInput<PlayerInputReader>();

            return UniTask.CompletedTask;
        }

        private void Release()
        {
            AudioManager.Instance.Release();

            TimeManager.Release();
            InputManager.Release();
            SceneManager.Release();
            PoolManager.Release();
            ResourceManager.Release();
        }

        private void Update()
        {
            InputManager.Update();
        }

        private void OnDestroy()
        {
            Release();
        }
    }
}
