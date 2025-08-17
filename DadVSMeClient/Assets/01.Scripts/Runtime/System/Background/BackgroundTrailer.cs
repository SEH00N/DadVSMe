using System;
using System.Collections.Generic;
using UnityEngine;
using H00N.Resources.Addressables;

namespace DadVSMe.Background
{
    public class BackgroundTrailer : MonoBehaviour
    {
        public event Action<int> onChangedTheme;

        private Queue<BackgroundThemeData> _themeDataQueue;
        private Transform _startTransform;
        private Transform _parentTransform;
        private Collider2D _boundary;

        private Queue<AddressableAsset<BackgroundObject>> _prefabContainer;
        private List<BackgroundObject> _runTimeBackgroundContainer;

        public void Initialize(BackgroundLayerInfo layerInfo, Collider2D boundary)
        {
            _themeDataQueue = new Queue<BackgroundThemeData>(layerInfo.themeDataArr);
            _startTransform = layerInfo.startTransform;
            _parentTransform = layerInfo.parentTransform;
            _boundary = boundary;

            _runTimeBackgroundContainer = new List<BackgroundObject>();
        }

        public void Run()
        {
            if(_themeDataQueue.Count == 0)
            {
                Debug.LogError("ThemeDataArray need initializing or not be assigned");
                return;
            }

            _prefabContainer = _themeDataQueue.Dequeue().GetBackgroundQueue();

            SpawnBackground(_prefabContainer.Dequeue(), _startTransform.position);
            var spawnedBG = _runTimeBackgroundContainer[0];
            SpawnBackground(_prefabContainer.Dequeue(), spawnedBG.SocketPosition);
        }

        private async void SpawnBackground(AddressableAsset<BackgroundObject> prefab, Vector2 spawnPosition)
        {
            await prefab.InitializeAsync();

            var bgObject = Instantiate(prefab.Asset, _parentTransform);
            bgObject.Initialize(spawnPosition);

            _runTimeBackgroundContainer.Add(bgObject);
        }

        private void DespawnBackground(BackgroundObject background)
        {
            Destroy(background.gameObject);
        }

        private void RefillPrefabContainer()
        {
            var newContainer = _themeDataQueue.Dequeue();
            _prefabContainer = newContainer.GetBackgroundQueue();

            onChangedTheme?.Invoke(newContainer.themeIdx);
        }

        private void FixedUpdate()
        {
            if (_runTimeBackgroundContainer.Count == 0) return;

            var firstBG = _runTimeBackgroundContainer[0];

            if (firstBG.SocketPosition.x < _boundary.bounds.min.x)
            {
                DespawnBackground(_runTimeBackgroundContainer[0]);
            }

            var lastIdx = _runTimeBackgroundContainer.Count - 1;
            var penultimateIdx = lastIdx - 1;

            var penultimateBG = _runTimeBackgroundContainer[penultimateIdx];

            if(penultimateBG.SocketPosition.x > _boundary.bounds.max.x)
            {
                if(_prefabContainer.Count == 0)
                {
                    RefillPrefabContainer();
                }

                var lastBG = _runTimeBackgroundContainer[lastIdx];
                SpawnBackground(_prefabContainer.Dequeue(), lastBG.SocketPosition);
            }
        }
    }
}
