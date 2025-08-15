using System;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class GuidedOrbSkill : AutoActiveSkill
    {
        private AddressableAsset<GuidedOrb> prefab = null;

        private float orbSpawnRadius = 3f;
        private float originCooltime;
        private int spawnCount = 1;

        public GuidedOrbSkill(float cooltime, AddressableAsset<GuidedOrb> prefab) : base(cooltime)
        {
            this.prefab = prefab;
            originCooltime = cooltime;
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

            LevelUp();
        }

        public override void LevelUp()
        {
            base.LevelUp();

            spawnCount = level;
            cooltime = Mathf.Clamp(originCooltime - (level - 1) * 0.3f, 1.5f, originCooltime); 
        }
    }
}