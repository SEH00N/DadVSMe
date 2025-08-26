using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.Enemies;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class EnemyWaveBehaviour : WaveBehaviour
    {
        [Serializable]
        public struct SpawnInfo
        {
            [Serializable]
            public struct SpawnTableData
            {
                public AddressableAsset<Unit> prefab;
                public AddressableAsset<EnemyDataBase> enemyData;
                public AddressableAsset<UnitStatData> statData;
                public Vector2 offset;
            }

            public Transform spawnPoint;
            public float conditionDistance;
            public List<SpawnTableData> spawnTable;
        }

        [SerializeField] List<SpawnInfo> spawnInfoList = new List<SpawnInfo>();
        private List<SpawnInfo> spawnInfoQueue = null;

        private void Awake()
        {
            foreach (SpawnInfo spawnInfo in spawnInfoList)
            {
                foreach (SpawnInfo.SpawnTableData spawnTable in spawnInfo.spawnTable)
                {
                    spawnTable.prefab.InitializeAsync().Forget();
                    spawnTable.enemyData.InitializeAsync().Forget();
                    spawnTable.statData.InitializeAsync().Forget();
                }
            }

            spawnInfoQueue = new List<SpawnInfo>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            spawnInfoQueue.Clear();
            spawnInfoQueue.AddRange(spawnInfoList);
        }

        protected override bool OnUpdate()
        {
            if(GameInstance.GameCycle == null || GameInstance.GameCycle.MainPlayer == null)
                return false;

            for(int i = spawnInfoQueue.Count - 1; i >= 0; i--)
            {
                SpawnInfo spawnInfo = spawnInfoQueue[i];
                Vector2 direction = GameInstance.GameCycle.MainPlayer.transform.position - spawnInfo.spawnPoint.position;
                if(direction.sqrMagnitude >= spawnInfo.conditionDistance * spawnInfo.conditionDistance)
                    continue;

                SpawnEnemy(spawnInfo);
                spawnInfoQueue.RemoveAt(i);
            }

            return spawnInfoQueue.Count == 0;
        }

        private void SpawnEnemy(SpawnInfo spawnInfo)
        {
            foreach (SpawnInfo.SpawnTableData spawnTable in spawnInfo.spawnTable)
            {
                Vector3 spawnPosition = spawnInfo.spawnPoint.position + (Vector3)spawnTable.offset;
                Unit enemy = PoolManager.Spawn<Unit>(spawnTable.prefab.Key, GameInstance.GameCycle.transform);
                enemy.transform.position = spawnPosition;
                enemy.FSMBrain.SetAIData(spawnTable.statData.Asset);
                enemy.Initialize(spawnTable.enemyData.Asset);
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (SpawnInfo spawnInfo in spawnInfoList)
            {
                if(spawnInfo.spawnPoint == null)
                    continue;

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(spawnInfo.spawnPoint.position, spawnInfo.conditionDistance);

                foreach (SpawnInfo.SpawnTableData spawnTable in spawnInfo.spawnTable)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(spawnInfo.spawnPoint.position + (Vector3)spawnTable.offset, 0.5f);
                }
            }
        }
        #endif
    }
}
