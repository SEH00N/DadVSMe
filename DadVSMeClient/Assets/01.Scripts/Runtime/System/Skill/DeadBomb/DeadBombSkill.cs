using System.Collections.Generic;
using DadVSMe.Entities;
using DadVSMe.Players.FSM;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    public class DeadBombSkill : UnitSkill
    {
        private AttackDataBase attackData;
        private AddressableAsset<PoolableEffect> bombEffect;
        private AddressableAsset<AudioClip> bombSound;
        private float attackRadius;
        private float levelUpIncreaseRate;

        public DeadBombSkill(AttackDataBase attackData, AddressableAsset<PoolableEffect> bombEffect,
            AddressableAsset<AudioClip> bombSound, float attackRadius, float levelUpIncreaseRate)
        {
            this.attackData = attackData;
            this.bombEffect = bombEffect;
            this.bombSound = bombSound;
            this.attackRadius = attackRadius;
            this.levelUpIncreaseRate = levelUpIncreaseRate;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        public override void Execute()
        {
           
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
                        Vector2 spawnPoint = target.transform.position;
                        Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, attackRadius);

                        foreach (var col in cols)
                        {
                            if (col.gameObject == ownerComponent.gameObject)
                                continue;

                            if (col.gameObject.TryGetComponent<UnitHealth>(out UnitHealth unitHealth))
                            {
                                unitHealth.Attack(ownerComponent.GetComponent<Unit>(), attackData);
                            }
                        }
                    }
                }

            _ = new PlayEffect(bombEffect, ownerComponent.transform.position, 1);
            _ = new PlaySound(bombSound);

        }

        public override void LevelUp()
        {
            base.LevelUp();

            attackRadius += levelUpIncreaseRate;
        }
    }
}
