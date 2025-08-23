using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class FirePunchSkill : UnitSkill
    {
        private static readonly Vector2 SpawnOffset = new Vector2(0f, 1.5f);

        private float levelUpIncreaseRate;
        private AddressableAsset<ParticleSystem> particlePrefab;

        public FirePunchSkill(float levelUpIncreaseRate, AddressableAsset<ParticleSystem> particlePrefab) : base()
        {
            this.levelUpIncreaseRate = levelUpIncreaseRate;
            this.particlePrefab = particlePrefab;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            Execute();
            SpawnEffectAsync();
        }

        private async void SpawnEffectAsync()
        {
            await particlePrefab.InitializeAsync();

            ParticleSystem particle = PoolManager.Spawn<ParticleSystem>(particlePrefab.Key, ownerComponent.gameObject.transform);
            particle.transform.localPosition = SpawnOffset;
            particle.Play();
        }

        public override void Execute()
        {
            ownerComponent.GetComponent<Unit>().SetAttackAttribute(EAttackAttribute.Fire);
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            ownerComponent.GetComponent<Unit>().SetAttackAttribute(EAttackAttribute.Normal);
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Unit owner = ownerComponent.GetComponent<Unit>();
            UnitStatData ownerStatData = owner.FSMBrain.GetAIData<UnitStatData>();
            UnitStat AttackPowerMultiplierStat = ownerStatData[EUnitStat.AttackPowerMultiplier];
            AttackPowerMultiplierStat.RegistAddModifier(levelUpIncreaseRate);
        }
    }
}