using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DadVSMe.Players.FSM;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class DeadBombSkill : UnitSkill
    {
        private AttackDataBase attackData;
        private AddressableAsset<PoolableEffect> effectRef;
        private float attackRadius;
        private float levelUpIncreaseRate;

        Unit attackTarget;
        Unit owner;

        public DeadBombSkill(AttackDataBase attackData, AddressableAsset<PoolableEffect> effectRef,
            float attackRadius, float levelUpIncreaseRate)
        {
            this.attackData = attackData;
            this.effectRef = effectRef;
            this.attackRadius = attackRadius;
            this.levelUpIncreaseRate = levelUpIncreaseRate;

            effectRef.InitializeAsync().Forget();
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        public override void Execute()
        {
            Vector2 spawnPoint = attackTarget.transform.position;
            Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, attackRadius);

            foreach (var col in cols)
            {
                if (col.gameObject == ownerComponent.gameObject)
                    continue;
                if (col.gameObject == attackTarget.gameObject)
                    continue;

                if (col.gameObject.TryGetComponent<UnitHealth>(out UnitHealth unitHealth))
                {
                    unitHealth.Attack(owner, attackData);
                }
            }

            _ = new PlayEffect(effectRef, spawnPoint, 1);

            attackTarget.OnDespawnEvent -= Execute;
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        private void OnAttackTarget(Unit target, IAttackData data)
        {
            FSMBrain playerBrain = ownerComponent.GetComponent<FSMBrain>();
            if (playerBrain.GetAIData<PlayerFSMData>().isAnger == false)
                return;

            if (target.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                if (targetHealth.CurrentHP <= 0f)
                {
                    attackTarget = target;

                    EntityAnimator animator = target.GetComponent<EntityAnimator>();
                    attackTarget.OnDespawnEvent += Execute;
                }
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();

            attackRadius += levelUpIncreaseRate;
        }
    }
}
