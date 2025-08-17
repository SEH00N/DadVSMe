using UnityEngine;
using System.Collections.Generic;
using H00N.Resources.Addressables;
using H00N.Resources;

namespace DadVSMe.Background
{
    [CreateAssetMenu(fileName = "BGThemeData", menuName = "DadVSMe/Background/ThemeData")]
    public class BackgroundThemeData : ScriptableObject
    {
        public int themeIdx;
        [SerializeField] List<AddressableAsset<BackgroundObject>> _backgroundList;

        public Queue<AddressableAsset<BackgroundObject>> GetBackgroundQueue()
        {
            return new Queue<AddressableAsset<BackgroundObject>>(_backgroundList);
        }

        public void ReleaseTheme()
        {
            foreach (var item in _backgroundList)
            {
                ResourceManager.ReleaseResource(item);
            }
        }
    }
}
