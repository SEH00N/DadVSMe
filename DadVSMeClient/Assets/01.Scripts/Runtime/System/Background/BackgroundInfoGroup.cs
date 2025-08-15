using H00N.Resources.Addressables;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Background
{
    public class BackgroundInfoGroup
    {
        public Queue<AddressableAsset<BackgroundObject>> container;
        public Transform startSpawnTransform;
        public float weight;

        public BackgroundInfoGroup(BackgroundData data, Transform transform)
        {
            container = new Queue<AddressableAsset<BackgroundObject>>(data.layers);
            weight = data.weight;
        }
    }
}
