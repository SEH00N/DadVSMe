using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DadVSMe.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn Info")]
        [SerializeField] AddressableAsset<Enemy> _enemyPrefab;
        [SerializeField] EnemySpawnData _enemySpawnData;

        [Header("Spawn Position Limit")]
        [SerializeField] Transform _backgroundLimitTrm;
        [SerializeField] Transform _deadlineTrm;

        [Header("Whether to spawn after the time limit")]
        [SerializeField] private bool _stopAfterTimeline = false;

        private CancellationTokenSource _canclelationTokenSource;
        private float _startTime;
        private int _onFieldEnemyCount;

        private Dictionary<IEntityData, int> _enemyCountDictionary;

        private void OnEnable()
        {
            _canclelationTokenSource = new CancellationTokenSource();
            _enemyCountDictionary = new Dictionary<IEntityData, int>();

            _startTime = Time.time;
            RunAsync(_canclelationTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            _canclelationTokenSource?.Cancel();
            _canclelationTokenSource?.Dispose();
            _canclelationTokenSource = null;
        }

        private async UniTaskVoid RunAsync(CancellationToken token)
        {
            if (_enemySpawnData == null)
            {
                Debug.LogWarning("[EnemySpawner] EnemySpawnData is null.");
                return;
            }

            if (_enemyPrefab != null)
            {
                try 
                { 
                    await _enemyPrefab.InitializeAsync().AttachExternalCancellation(token); 
                }
                catch (OperationCanceledException) 
                { 
                    return; 
                }
            }

            while (token.IsCancellationRequested == false)
            {
                var (phase, phaseIndex) = GetCurrentPhase();

                if (phase == null)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    continue;
                }

                var delaySec = Mathf.Max(0.01f, phase.spawnInterval.Next());

                bool canSpawn = phase.totalEnemyCountOnField <= 0 || _onFieldEnemyCount < phase.totalEnemyCountOnField;

                if (canSpawn)
                {
                    var unit = PickUnitForPhase(phase, _enemyCountDictionary);
                    await SpawnFromUnitAsync(unit, token);
                }

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delaySec), cancellationToken: token);
                }
                catch (OperationCanceledException) 
                { 
                    break; 
                }
            }
        }

        private (SpawnPhase phase, int index) GetCurrentPhase()
        {
            var data = _enemySpawnData;

            if (data.enemySawnPhaseList == null || data.enemySawnPhaseList.Count == 0)
                return (null, -1);

            int count = Mathf.Max(1, data.divedCount);
            int total = Mathf.Max(1, data.totalDurationSeconds);

            var segments = data.useCustomRanges
                ? ReadCustomSegments(data.customRanges, total)
                : ComputeRoundedSegments(count, total);

            int usable = Mathf.Min(segments.Count, data.enemySawnPhaseList.Count);

            if (usable == 0) 
                return (null, -1);

            float elapsed = Time.time - _startTime;

            if (elapsed >= total)
            {
                if (_stopAfterTimeline) 
                    return (null, -1);

                return (data.enemySawnPhaseList[usable - 1], usable - 1);
            }

            int sec = Mathf.Max(0, Mathf.FloorToInt(elapsed));
            for (int i = 0; i < usable; i++)
            {
                if (sec >= segments[i].start && sec < segments[i].end)
                    return (data.enemySawnPhaseList[i], i);
            }

            return (data.enemySawnPhaseList[usable - 1], usable - 1);
        }

        private static List<(int start, int end)> ReadCustomSegments(List<SegmentRange> ranges, int total)
        {
            var res = new List<(int, int)>(ranges != null ? ranges.Count : 0);

            if (ranges == null || ranges.Count == 0) 
            { 
                res.Add((0, total)); 
                return res; 
            }

            for (int i = 0; i < ranges.Count; i++)
            {
                int s = Mathf.Clamp(ranges[i].startSec, 0, total);
                int e = Mathf.Clamp(ranges[i].endSec, 0, total);
                if (e < s) e = s;
                res.Add((s, e));
            }

            return res;
        }

        private static List<(int start, int end)> ComputeRoundedSegments(int n, int totalSeconds)
        {
            var result = new List<(int, int)>(n);

            if (n <= 0) 
            { 
                result.Add((0, totalSeconds)); 
                return result; 
            }

            var bounds = new int[n + 1];
            for (int i = 0; i <= n; i++)
            {
                double raw = (double)i * totalSeconds / n;
                bounds[i] = (int)Math.Round(raw, MidpointRounding.AwayFromZero);
            }

            bounds[0] = 0;
            bounds[n] = totalSeconds;

            for (int i = 1; i <= n; i++)
            {
                if (bounds[i] <= bounds[i - 1])
                    bounds[i] = Math.Min(totalSeconds, bounds[i - 1] + 1);
            }

            bounds[n] = totalSeconds;

            for (int i = 0; i < n; i++)
                result.Add((bounds[i], bounds[i + 1]));

            return result;
        }

        private IEntityData PickUnitForPhase(SpawnPhase phase, IReadOnlyDictionary<IEntityData, int> currentCounts)
        {
            var list = phase.enemiesList;
            if (list == null || list.Count == 0) return null;

            List<IEntityData> allowedunitList = null;
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                int cur = 0;
                if (currentCounts != null && currentCounts.TryGetValue(e.enemyData, out var v)) cur = v;

                bool ok = (e.maxOnField <= 0) || (cur < e.maxOnField);
                if (ok)
                {
                    (allowedunitList ??= new List<IEntityData>(list.Count)).Add(e.enemyData);
                }
            }

            if (allowedunitList == null || allowedunitList.Count == 0)
                return null; // 모두 상한 도달

            int pick = UnityEngine.Random.Range(0, allowedunitList.Count);
            return allowedunitList[pick];
        }

        private async UniTask SpawnFromUnitAsync(IEntityData unitData, CancellationToken token)
        {
            if(unitData == null) return;

            try 
            { 
                await _enemyPrefab.InitializeAsync().AttachExternalCancellation(token); 
            }
            catch (OperationCanceledException) 
            {
                return; 
            }

            Enemy enemy = PoolManager.Spawn<Enemy>(resourceName: _enemyPrefab);
            enemy.Initialize(unitData);
            enemy.transform.position = GetSpawnPos(Camera.main, _backgroundLimitTrm, _deadlineTrm);

            CountingEnemy(enemy, unitData);
        }

        private void CountingEnemy(Enemy enemy, IEntityData unitData)
        {
            if (_enemyCountDictionary.TryGetValue(unitData, out int count))
            {
                ++count;
                _enemyCountDictionary[unitData] = count;
            }
            else
            {
                _enemyCountDictionary.Add(unitData, 1);
            }

            ++_onFieldEnemyCount;

            enemy.onDespawned += HandleDecountEnemy;
        }

        private void HandleDecountEnemy(Enemy enemy)
        {
            enemy.onDespawned -= HandleDecountEnemy;

            var enemyData = enemy.DataInfo;

            if (_enemyCountDictionary.TryGetValue(enemyData, out int count))
            {
                --count;
                _enemyCountDictionary[enemyData] = count;
            }
            else
            {
                Debug.LogError("Not Register Enemy");
            }

            --_onFieldEnemyCount;
        }

        private Vector3 GetSpawnPos(Camera cam, Transform yAnchor, Transform xAnchor)
        {
            float zDist = Mathf.Abs(cam.transform.position.z);

            // 화면 아래(-0.1 뷰포트)에서 임의 X로 샘플
            Vector3 p = cam.ViewportToWorldPoint(new Vector3(Random.value, -0.1f, zDist));
            p.z = 0f;

            // 엄격히 아래/왼쪽으로 고정
            p.y = Mathf.Min(p.y, yAnchor.position.y - 0.0001f);
            p.x = Mathf.Min(p.x, xAnchor.position.x - 0.0001f);

            return p;
        }
    }
}
