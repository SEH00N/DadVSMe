using System;
using System.Collections.Generic;
using UnityEngine;
using DadVSMe.Entities;

namespace DadVSMe
{
    // ���� �ð��� ���� ���� �󵵰� �ٸ���.
    // �����Ǵ� ���� ������ �ʿ� �����ϴ� ���� ������ ���� ���� �����ȴ�.
    // ���� ������ ���� �ʵ忡 ������ �� �ִ� �ִ� ���� �ٸ���.
    // �ʵ忡 �����ϴ� ��� ���� ���� �ִ�ġ�� ���� �� ����.

    [Serializable]
    public class SpawnPhase
    {
        [Header("Spawn Interval (Sec)")]
        public RandomRangeFloat spawnInterval = new RandomRangeFloat(1.0f, 2.5f);

        [Header("Spawnable Enemy Data")]
        public List<UnitData> enemiesDataList;

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
