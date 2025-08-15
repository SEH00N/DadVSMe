using UnityEngine;
using System.Collections.Generic;
using H00N.Resources.Addressables;
using H00N.Resources;

namespace DadVSMe.Background
{
    public class BackgroundManagement : MonoBehaviour 
    {
        private Dictionary<float, BackgroundInfoGroup> _backgroundInfoContainer;
        private Dictionary<float, List<BackgroundObject>> _backgroundRuntimeContainer;
        private Dictionary<BackgroundObject, AddressableAsset<BackgroundObject>> _assetMap;

        private Collider2D _boundary;
        private Transform _cameraTransform;

        public void Initialize(Collider2D boundary, Transform cameraTrm)
        {
            _backgroundInfoContainer = new();
            _backgroundRuntimeContainer = new();
            _assetMap = new();

            _boundary = boundary;
            _cameraTransform = cameraTrm;
        }

        public void RegisterBackgroundData(BackgroundData data, Transform startTrm)
        {
            var group = new BackgroundInfoGroup(data, startTrm);
            var weight = group.weight;

            _backgroundInfoContainer.Add(weight, group);

            SpawnBackground(group.container.Dequeue(), weight, group.startSpawnTransform.position);
        }

        private async void SpawnBackground(AddressableAsset<BackgroundObject> bgPrefab, float weight, Vector2 spawnPos)
        {
            if(bgPrefab.Initialized == false)
            {
                await bgPrefab.InitializeAsync();
            }

            var currentBG = Instantiate(ResourceManager.GetResource<BackgroundObject>(bgPrefab));
            currentBG.Initialize(_cameraTransform, weight, spawnPos);

            if (_backgroundRuntimeContainer.ContainsKey(weight) == false)
            {
                _backgroundRuntimeContainer.Add(weight, new List<BackgroundObject>());
            }

            _backgroundRuntimeContainer[weight].Add(currentBG);
            _assetMap.Add(currentBG, bgPrefab);
        }

        private void ReleaseBackground(BackgroundObject bg)
        {
            var asset = _assetMap[bg];
            _assetMap.Remove(bg);

            Destroy(bg.gameObject);
            ResourceManager.ReleaseResource(asset);
        }

        private void FixedUpdate()
        {
            float boundaryMin = _boundary.bounds.min.x;
            float boundaryMax = _boundary.bounds.max.x;

            foreach(var pair in _backgroundRuntimeContainer)
            {
                var runtimeBGList = pair.Value;

                if (runtimeBGList[0].SocketPosition.x < boundaryMin)
                {
                    var bg = runtimeBGList[0];
                    runtimeBGList.Remove(bg);

                    ReleaseBackground(bg);
                }

                var lastIdx = pair.Value.Count - 1;
                var penultimateIdx = lastIdx - 1;

                if (runtimeBGList[penultimateIdx].SocketPosition.x > boundaryMax)
                {
                    var weight = pair.Key;
                    var info = _backgroundInfoContainer[pair.Key];

                    var spawnPos = runtimeBGList[lastIdx].SocketPosition;

                    SpawnBackground(info.container.Dequeue(), weight, spawnPos);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var kv in _backgroundRuntimeContainer)
            {
                var list = kv.Value;
                if (list == null) continue;
                for (int i = 0; i < list.Count; i++)
                {
                    var bg = list[i];
                    if (bg != null) ReleaseBackground(bg);
                }
                list.Clear();
            }

            _backgroundRuntimeContainer.Clear();
            _assetMap.Clear();
        }
    }
}
