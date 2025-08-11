using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DadVSMe.Enemies;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DadVSMe
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private AddressableAsset<Enemy> _enemyPrefab;
        [SerializeField] private EnemySpawnData _enemySpawnData;

        [SerializeField] private bool _stopAfterTimeline = false;

        private CancellationTokenSource _canclelationTokenSource;
        private float _startTime;
        private int _activeOnField;

        private void OnEnable()
        {
            _canclelationTokenSource = new CancellationTokenSource();
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

            if (_enemyPrefab != null && _enemyPrefab.Initialized == false)
            {
                try { await _enemyPrefab.InitializeAsync().AttachExternalCancellation(token); }
                catch (OperationCanceledException) { return; }
            }

            while (token.IsCancellationRequested == false)
            {
                var (phase, phaseIndex) = GetCurrentPhase();
                if (phase == null)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    continue;
                }

                var delaySec = Mathf.Max(0.01f, Random.Range(phase.intervalMin, phase.intervalMax));

                bool canSpawn = phase.totalEnemyCountOnField <= 0 || _activeOnField < phase.totalEnemyCountOnField;

                if (canSpawn)
                {
                    var unit = PickUnitForPhase(phase);
                    await SpawnFromUnitAsync(unit, token);
                }

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delaySec), cancellationToken: token);
                }
                catch (OperationCanceledException) { break; }
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
            if (usable == 0) return (null, -1);

            float elapsed = Time.time - _startTime;

            if (elapsed >= total)
            {
                if (_stopAfterTimeline) return (null, -1);

                return (data.enemySawnPhaseList[usable - 1], usable - 1);
            }

            int sec = Mathf.Max(0, Mathf.FloorToInt(elapsed));
            for (int i = 0; i < usable; i++)
            {
                // [start, end) ±¸°£
                if (sec >= segments[i].start && sec < segments[i].end)
                    return (data.enemySawnPhaseList[i], i);
            }

            return (data.enemySawnPhaseList[usable - 1], usable - 1);
        }

        private static List<(int start, int end)> ReadCustomSegments(List<SegmentRange> ranges, int total)
        {
            var res = new List<(int, int)>(ranges != null ? ranges.Count : 0);
            if (ranges == null || ranges.Count == 0) { res.Add((0, total)); return res; }

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
            if (n <= 0) { result.Add((0, totalSeconds)); return result; }

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

        private UnitData PickUnitForPhase(SpawnPhase phase)
        {
            var list = phase.enemiesDataList;
            if (list == null || list.Count == 0) return null;
            int idx = UnityEngine.Random.Range(0, list.Count);
            return list[idx];
        }

        private async UniTask SpawnFromUnitAsync(UnitData unit, CancellationToken token)
        {
            if (_enemyPrefab.Initialized == false)
            {
                try { await _enemyPrefab.InitializeAsync().AttachExternalCancellation(token); }
                catch (OperationCanceledException) { return; }
            }

            Enemy enemy = PoolManager.Spawn<Enemy>(_enemyPrefab);
        }
    }
}
