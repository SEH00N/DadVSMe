using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public struct ReleaseResourceByLabel
    {
        public async UniTask ReleaseAsync(string label)
        {
            List<string> resources = await AddressableResourceLoader.ReleaseResourcesByLabelAsync(label);
            foreach (string resource in resources)
                PoolManager.RemovePool(resource);
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