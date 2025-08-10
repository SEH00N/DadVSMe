using System;
using System.Collections.Generic;
using UnityEngine;
using DadVSMe.Enemies;
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
        [Min(0.01f)] public float intervalMin = 1.0f;
        [Min(0.01f)] public float intervalMax = 2.5f;

        [Header("Spawnable Enemy Data")]
        public List<UnitData> enemiesDataList = new ();

        [Header("Max Enemy Count on Field")]
        [Min(0)] public int totalEnemyCountOnField = 0;
    }

    [CreateAssetMenu(fileName = "EnemySpawnData", menuName = "CreateSO/Entity/EnemySpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        
    }
}
