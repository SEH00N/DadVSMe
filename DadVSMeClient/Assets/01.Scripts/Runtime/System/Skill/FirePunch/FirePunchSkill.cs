using System;
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
        private AddressableAsset<Fire> firePrefab;
        private float burnTime;
        private float attackDelay;
        private AttackDataBase attackData;

        private Unit owner;

        public FirePunchSkill(AttackDataBase attackData, float levelUpIncreaseRate, AddressableAsset<Fire> firePrefab, float burnTime, float attackDelay) : base()
        {
            this.levelUpIncreaseRate = levelUpIncreaseRate;
            this.firePrefab = firePrefab;
            this.burnTime = burnTime;
            this.attackDelay = attackDelay;
            this.attackData = attackData;

            firePrefab.InitializeAsync().Forget();
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
            Execute();
        }

        private void OnAttackTarget(Unit target, IAttackData attackData)
        {
            Fire fire = PoolManager.Spawn(firePrefab).GetComponent<Fire>();
            fire.Init(ownerComponent.GetComponent<Unit>(), target, this.attackData, burnTime, attackDelay);
        }

        public override void Execute()
        {
            owner.SetAttackAttribute(EAttackAttribute.Fire);
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            owner.SetAttackAttribute(EAttackAttribute.Normal);
            owner.onAttackTargetEvent.RemoveListener(OnAttackTarget);
        }

        public override void LevelUp()
        {
            base.LevelUp();

            burnTime += levelUpIncreaseRate;
        }
    }
}