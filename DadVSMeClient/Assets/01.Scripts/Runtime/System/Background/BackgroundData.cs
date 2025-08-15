using UnityEngine;
using System.Collections.Generic;
using H00N.Resources.Addressables;

namespace DadVSMe.Background
{
    [CreateAssetMenu(fileName = "BackgroundData", menuName = "CreateSO/Background/Data")]
    public class BackgroundData : ScriptableObject
    {
        public List<AddressableAsset<BackgroundObject>> layers;
        public float weight;
    }
}
