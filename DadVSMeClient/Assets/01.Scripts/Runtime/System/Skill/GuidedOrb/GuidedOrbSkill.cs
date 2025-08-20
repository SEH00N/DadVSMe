using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class GuidedOrbSkill : AutoActiveSkill
    {
        private AddressableAsset<GuidedOrb> prefab = null;

        private float orbSpawnRadius;
        private float originCooltime;
        private int spawnCount;
        private int levelUpIncreaseRate;

        public GuidedOrbSkill(AddressableAsset<GuidedOrb> prefab, float cooltime, int levelUpIncreaseRate) : base(cooltime)
        {
            prefab.InitializeAsync().Forget();

            this.prefab = prefab;
            this.levelUpIncreaseRate = levelUpIncreaseRate;
            originCooltime = cooltime;
            orbSpawnRadius = 3f;
            spawnCount = 2;
        }

        public override void Execute()
        {
            base.Execute();

            float angle = 360f / spawnCount;
            float currentAngle = 0f;
            while (currentAngle < 360f)
            {
                currentAngle += angle;
                Vector2 spawnPoint = ownerComponent.transform.position +
                    new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad)) * orbSpawnRadius;
                Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, 10f);


                if (cols.Length == 0)
                    continue;

                Unit target = null;
                foreach (var col in cols)
                {
                    if (col.gameObject == ownerComponent.gameObject)
                        continue;

                    if (col.gameObject.TryGetComponent<Unit>(out Unit unit))
                    {
                        target = unit;
                        break;
                    }
                }

                if (target == null)
                    continue;

                GuidedOrb guidedOrb = PoolManager.Spawn<GuidedOrb>(prefab);
                guidedOrb.transform.position = spawnPoint;
                guidedOrb.SetInstigator(ownerComponent.GetComponent<Unit>());
                guidedOrb.SetTarget(target);
                guidedOrb.Launch();
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();

            spawnCount += levelUpIncreaseRate;
        }
    }
}