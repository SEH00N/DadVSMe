using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class GuidedOrbSkill : AutoActiveSkill<GuidedOrbSkillData, GuidedOrbSkillData.Option>
    {
        private const float ORB_SPAWN_RADIUS = 3f;

        // private AddressableAsset<GuidedOrb> prefab = null;
        // private AddressableAsset<AudioClip> sound;

        // private float orbSpawnRadius;
        // private float originCooltime;
        // private int spawnCount;
        // private int levelUpIncreaseRate;
        // private AttackDataBase attackData;

        // public GuidedOrbSkill(AddressableAsset<GuidedOrb> prefab, float cooltime, int levelUpIncreaseRate,
        //     AddressableAsset<AudioClip> sound, AttackDataBase attackData) : base(cooltime)
        // {
        //     prefab.InitializeAsync().Forget();
        //     sound.InitializeAsync().Forget();

        //     this.prefab = prefab;
        //     this.sound = sound;
        //     this.attackData = attackData;
        //     this.levelUpIncreaseRate = levelUpIncreaseRate;
        //     originCooltime = cooltime;
        //     orbSpawnRadius = 3f;
        //     spawnCount = 2;
        // }

        public override async void Execute()
        {
            GuidedOrbSkillData data = GetData();
            GuidedOrbSkillData.Option option = GetOption();

            AttackDataBase attackData = data.attackData;
            AddressableAsset<GuidedOrb> prefab = data.prefab;
            int spawnCount = option.spawnCount;
            int damage = option.damage;

            await prefab.InitializeAsync();
            
            float angle = 360f / spawnCount;
            float currentAngle = 0f;

            DynamicAttackData dynamicAttackData = new DynamicAttackData(attackData);
            dynamicAttackData.SetDamage(damage);

            while (currentAngle < 360f)
            {
                currentAngle += angle;
                Vector2 spawnPoint = ownerComponent.transform.position + new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad)) * ORB_SPAWN_RADIUS;
                Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, 10f);

                if (cols.Length == 0)
                    continue;

                Unit target = null;
                foreach (var col in cols)
                {
                    if (col.gameObject == ownerComponent.gameObject)
                        continue;

                    if (col.gameObject.TryGetComponent<Unit>(out Unit unit) == false)
                        continue;
                    
                    // Do not targeting grabbed or floated enemy
                    if(unit.StaticEntity || unit.UnitHealth.CurrentHP <= 0)
                        continue;

                    target = unit;
                    break;
                }

                if (target == null)
                    continue;

                GuidedOrb guidedOrb = PoolManager.Spawn<GuidedOrb>(prefab.Key, GameInstance.GameCycle.transform);
                guidedOrb.transform.position = spawnPoint;
                guidedOrb.SetInstigator(ownerComponent.GetComponent<Unit>(), dynamicAttackData);
                guidedOrb.SetTarget(target);
                guidedOrb.Launch();
            }

            _ = new PlayAttackFeedback(attackData, EAttackAttribute.Crazy, ownerComponent.transform.position, Vector3.zero, 1);
            _ = new PlayAttackSound(attackData, EAttackAttribute.Crazy);
        }

        protected override float GetCoolTime()
        {
            return GetOption().coolTime;
        }
    }
}