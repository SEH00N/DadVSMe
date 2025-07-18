using System.Collections.Generic;
using DadVSMe.Inputs;
using H00N.Extensions;
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
            managerList.Add(gameObject.GetOrAddComponent<GlobalCoroutineManager>());
            managerList.ForEach(manager => manager.Initialize());
        }

        private void Release()
        {
            foreach(IManager manager in managerList)
                manager.Release();

            managerList.Clear();
        }

        private void Update()
        {
            InputManager.Update();
        }
    }
}
