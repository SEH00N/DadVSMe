using System.Collections.Generic;
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

        private List<IManager> managerList = null;

        private void Awake()
        {
            if(Instance != null)
            {
                Instance.Release();
                DestroyImmediate(Instance.gameObject);
            }

            Instance = this;
            managerList = new List<IManager>();
            Initialize();
        }

        private void Initialize()
        {
            ResourceManager.Initialize(new AddressableResourceLoader());
            PoolManager.Initialize(transform);

            managerList.Add(gameObject.GetOrAddComponent<GlobalCoroutineManager>());
            managerList.ForEach(manager => manager.Initialize());

            Application.targetFrameRate = 60;

            InputManager.ChangeInput<PlayerInputReader>();
        }

        private void Release()
        {
            foreach (IManager manager in managerList)
                manager.Release();

            managerList.Clear();

            PoolManager.Release();
            ResourceManager.Release();
        }

        private void Update()
        {
            InputManager.Update();
        }
    }
}
