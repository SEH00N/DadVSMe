using System;
using System.Collections.Generic;
using UnityEngine;
using DadVSMe.Enemies;

namespace DadVSMe.Enemies
{
    // 현재 시간에 따라 적 스폰 빈도가 다르다.
    // 스폰되는 적의 종류는 맵에 존재하는 적의 종류의 수에 따라 결정된다. (08.12. 아직 미구현)
    // 적의 종류마다 필드에 존재할 수 있는 최대 수가 다르다.
    // 필드에 존재하는 모든 적의 합은 최대치를 넘을 수 없다.

    [Serializable]
    public class EnemyEntry
    {
        public EnemyDataBase enemyData;
        [Min(0)] public int maxOnField = 0; // 0 = unlimited
    }

    [Serializable]
    public class SpawnPhase
    {
        [Header("Spawn Interval (Sec)")]
        public RandomRangeFloat spawnInterval = new RandomRangeFloat(1.0f, 2.5f);

        [Header("Spawnable Enemy Data")]
        public List<EnemyEntry> enemiesList;

        [Header("Max Enemy Count on Field")]
        [Min(0)] public int totalEnemyCountOnField = 0;
    }

    [Serializable]
    public class SegmentRange
    {
        [Min(0)] public int startSec = 0;
        [Min(0)] public int endSec = 0;
    }

    [CreateAssetMenu(fileName = "EnemySpawnData", menuName = "CreateSO/Entity/EnemySpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        [Header("Divide count for phases (>=1)")]
        [Min(1)] public int divedCount = 2;

        [Header("Total duration in seconds (default 10 min = 600)")]
        [Min(1)] public int totalDurationSeconds = 600;

        [Header("Use custom time ranges instead of equal division")]
        public bool useCustomRanges = false;

        [Header("Custom ranges (size must match divedCount)")]
        public List<SegmentRange> customRanges = new List<SegmentRange>();

        [Header("Phases (size auto-synced with divedCount)")]
        public List<SpawnPhase> enemySawnPhaseList = new List<SpawnPhase>();
    }
}
