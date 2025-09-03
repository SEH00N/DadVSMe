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
    public class DeadBombSkill : UnitSkill<DeadBombSkillData, DeadBombSkillData.Option>
    {
        private Unit attackTarget;
        private Unit owner;

        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            base.OnRegist(ownerComponent, skillData);

            _ = new InitializeAttackFeedback(GetData().attackData);
            
            owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        public override void Execute()
        {
            DeadBombSkillData data = GetData();

            AttackDataBase attackData = data.attackData;
            float attackRadius = data.attackRadius;
            int additiveDamage = GetOption().additiveDamage;

            Vector2 spawnPoint = attackTarget.transform.position;
            Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, attackRadius);

            UnitFSMData unitFSMData = owner.FSMBrain.GetAIData<UnitFSMData>();
            EAttackAttribute attackAttribute = unitFSMData.attackAttribute;
            unitFSMData.attackAttribute = EAttackAttribute.Fire;

            foreach (var col in cols)
            {
                if (col.gameObject == ownerComponent.gameObject)
                    continue;
                if (col.gameObject == attackTarget.gameObject)
                    continue;

                if (col.gameObject.TryGetComponent<IHealth>(out IHealth unitHealth))
                {
                    DynamicAttackData dynamicAttackData = new DynamicAttackData(attackData);
                    dynamicAttackData.SetDamage(dynamicAttackData.Damage + additiveDamage);
                    unitHealth.Attack(owner, dynamicAttackData);
                    _ = new PlayHitFeedback(dynamicAttackData, unitFSMData.attackAttribute, unitHealth.Position, Vector3.zero, unitFSMData.forwardDirection);
                }
            }
            
            _ = new PlayAttackFeedback(attackData, unitFSMData.attackAttribute, spawnPoint, Vector3.zero, unitFSMData.forwardDirection);
            _ = new PlayAttackSound(attackData, unitFSMData.attackAttribute);
            // _ = new PlayEffect(effectRef, spawnPoint, 1);
            // _ = new PlaySound(soundRef);

            unitFSMData.attackAttribute = attackAttribute;

            attackTarget.OnDespawnEvent -= Execute;
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.RemoveListener(OnAttackTarget);
        }

        private void OnAttackTarget(Unit target, IAttackData data)
        {
            FSMBrain playerBrain = ownerComponent.GetComponent<FSMBrain>();
            if (playerBrain.GetAIData<PlayerFSMData>().isAnger == false)
                return;

            if (target.TryGetComponent<IHealth>(out IHealth targetHealth))
            {
                if (targetHealth.CurrentHP <= 0f)
                {
                    attackTarget = target;

                    EntityAnimator animator = target.GetComponent<EntityAnimator>();
                    attackTarget.OnDespawnEvent += Execute;
                }
            }
        }
    }
}
