using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace DadVSMe
{
    public static class SceneManager
    {
        private static Dictionary<string, AsyncOperationHandle<SceneInstance>> loadedSceneTable = null;

        public static void Initialize()
        {
            loadedSceneTable = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
        }

        public static async void Release()
        {
            foreach(var sceneHandle in loadedSceneTable.Values)
            {
                if (sceneHandle.IsValid())
                    await Addressables.UnloadSceneAsync(sceneHandle);

                sceneHandle.Release();
            }
            loadedSceneTable.Clear();
            loadedSceneTable = null;
        }

        public static async UniTask<bool> TryLoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            if(loadedSceneTable.ContainsKey(sceneName))
                return false;

            AsyncOperationHandle<SceneInstance> sceneHandle = Addressables.LoadSceneAsync(sceneName, loadSceneMode);
            await sceneHandle;

            if(sceneHandle.Status == AsyncOperationStatus.Failed)
                return false;

            loadedSceneTable.Add(sceneName, sceneHandle);
            return true;
        }

        public static async UniTask<bool> TryUnloadSceneAsync(string sceneName)
        {
            if(loadedSceneTable.TryGetValue(sceneName, out AsyncOperationHandle<SceneInstance> sceneHandle))
                return false;

            if(sceneHandle.IsValid() == false)
            {
                loadedSceneTable.Remove(sceneName);
                return false;
            }

            AsyncOperationHandle<SceneInstance> unloadSceneHandle = Addressables.UnloadSceneAsync(sceneHandle);
            await unloadSceneHandle;

            if(unloadSceneHandle.Status == AsyncOperationStatus.Failed)
                return false;

            loadedSceneTable.Remove(sceneName);
            return true;
        }
    }
}
