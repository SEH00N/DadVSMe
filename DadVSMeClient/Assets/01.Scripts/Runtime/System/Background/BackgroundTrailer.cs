using System;
using System.Collections.Generic;
using UnityEngine;
using H00N.Resources.Addressables;
using Cysharp.Threading.Tasks;

namespace DadVSMe.Background
{
    public class BackgroundTrailer : MonoBehaviour
    {
        public event Action<int> onDespawnedBackground;

        private Transform _startTransform;
        private Transform _parentTransform;
        private Transform _cameraTransform;
        private Collider2D _boundary;

        private Queue<BackgroundThemeData> _themeDataQueue;
        private BackgroundThemeData _currentThemeData;
        private int _latestDespawnedThemeIdx;

        private Queue<AddressableAsset<BackgroundObject>> _prefabContainer;
        private List<BackgroundObject> _runTimeBackgroundContainer;

        private bool _onRunning;
        private bool _onSpawnning;

        public void Initialize(BackgroundLayerInfo layerInfo, Transform cameraTrm, Collider2D boundary)
        {
            _themeDataQueue = new Queue<BackgroundThemeData>(layerInfo.themeDataArr);
            _startTransform = layerInfo.startTransform;
            _parentTransform = layerInfo.parentTransform;
            _cameraTransform = cameraTrm;
            _boundary = boundary;

            _runTimeBackgroundContainer = new List<BackgroundObject>();
            _onRunning = false;
        }

        public void Run()
        {
            if(_themeDataQueue.Count == 0)
            {
                Debug.LogError("ThemeDataArray need initializing or not be assigned");
                return;
            }

            RefillPrefabContainer();
            _latestDespawnedThemeIdx = _currentThemeData.themeIdx;

            SpawnBackground(_prefabContainer.Dequeue(), _startTransform.position);
            var spawnedBG = _runTimeBackgroundContainer[0];
            SpawnBackground(_prefabContainer.Dequeue(), spawnedBG.SocketPosition);

            _onRunning = true;
        }

        private async void SpawnBackground(AddressableAsset<BackgroundObject> prefab, Vector2 spawnPosition)
        {
            _onSpawnning = true;
            await prefab.InitializeAsync();

            var bgObject = Instantiate(prefab.Asset, _parentTransform);
            bgObject.Initialize(spawnPosition, _cameraTransform, _currentThemeData.themeIdx);

            _runTimeBackgroundContainer.Add(bgObject);
            _onSpawnning = false;
        }

        private void DespawnBackground(BackgroundObject background)
        {
            _runTimeBackgroundContainer.Remove(background);
            Destroy(background.gameObject);

            Debug.Log(_latestDespawnedThemeIdx);

            if(_latestDespawnedThemeIdx != background.ThemeIdx)
            {
                onDespawnedBackground?.Invoke(_latestDespawnedThemeIdx);
                _latestDespawnedThemeIdx = background.ThemeIdx;
            }
        }

        private void RefillPrefabContainer()
        {
            if(_themeDataQueue.Count == 0)
            {
                Debug.LogError("No more BackgroundData");
                return;
            }

            _currentThemeData = _themeDataQueue.Peek();
            _prefabContainer = _themeDataQueue.Dequeue().GetBackgroundQueue();
        }

        private void FixedUpdate()
        {
            if (_onRunning == false) return;
            if (_runTimeBackgroundContainer.Count < 1) return;
            if (_onSpawnning) return;

            var firstBG = _runTimeBackgroundContainer[0];

            if (firstBG.SocketPosition.x < _boundary.bounds.min.x)
            {
                DespawnBackground(_runTimeBackgroundContainer[0]);
            }

            var lastIdx = _runTimeBackgroundContainer.Count - 1;
            var penultimateIdx = lastIdx - 1;
            var penultimateBG = _runTimeBackgroundContainer[penultimateIdx];

            if (penultimateBG.SocketPosition.x < _boundary.bounds.max.x)
            {
                if (_prefabContainer.Count == 0)
                {
                    RefillPrefabContainer();
                }

                if (_prefabContainer.Count == 0) return;

                var lastBG = _runTimeBackgroundContainer[lastIdx];
                SpawnBackground(_prefabContainer.Dequeue(), lastBG.SocketPosition);
            }
        }
    }
}
