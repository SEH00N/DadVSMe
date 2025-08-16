using DadVSMe.Inputs;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

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
            Initialize();
        }

        private void Initialize()
        {
            ResourceManager.Initialize(new AddressableResourceLoader());
            PoolManager.Initialize(transform);

            gameObject.GetOrAddComponent<AudioManager>().Initialize();

            Application.targetFrameRate = 60;

            InputManager.ChangeInput<PlayerInputReader>();
        }

        private void Release()
        {
            AudioManager.Instance.Release();

            PoolManager.Release();
            ResourceManager.Release();
        }

        private void Update()
        {
            InputManager.Update();
        }
    }
}
