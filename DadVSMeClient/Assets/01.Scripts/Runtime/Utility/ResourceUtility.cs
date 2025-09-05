using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    public struct ReleaseResourceByLabel
    {
        public UniTask ReleaseAsync(string label)
        {
            // List<string> resources = new List<string>();
            // await AddressableResourceLoader.ReleaseResourcesByLabelAsync(label, resources);
            // foreach (string resource in resources)
            //     PoolManager.RemovePool(resource);
            return UniTask.CompletedTask;
        }
    }

    public struct LoadResourceByLabel
    {
        public async UniTask LoadAsync(string label)
        {
            await AddressableResourceLoader.LoadResourcesByLabelAsync<Sprite>(label);
            await AddressableResourceLoader.LoadResourcesByLabelAsync(label);
        }
    }
}