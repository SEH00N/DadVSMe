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
        private AddressableAsset<Bomb> bombRef;
        private float attackRadius;
        private float levelUpIncreaseRate;

        Unit attackTarget;

        public DeadBombSkill(AttackDataBase attackData, AddressableAsset<Bomb> bombRef,
            float attackRadius, float levelUpIncreaseRate)
        {
            this.attackData = attackData;
            this.bombRef = bombRef;
            this.attackRadius = attackRadius;
            this.levelUpIncreaseRate = levelUpIncreaseRate;

            bombRef.InitializeAsync().Forget();
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        public override void Execute()
        {
            Vector2 spawnPoint = attackTarget.transform.position;
            Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, attackRadius);
            UnitHealth target = null;
            Debug.Log(cols.Length);
            foreach (var col in cols)
            {
                if (col.gameObject == ownerComponent.gameObject)
                    continue;

                if (col.gameObject.TryGetComponent<UnitHealth>(out UnitHealth unitHealth))
                {
                    if (target == null)
                    {
                        target = unitHealth;
                    }
                    else
                    {
                        if (target.CurrentHP < unitHealth.CurrentHP)
                        {
                            target = unitHealth;
                        }
                    }
                }
            }

            if (target != null)
            {
                Bomb bomb = PoolManager.Spawn(bombRef).GetComponent<Bomb>();
                bomb.transform.position = attackTarget.transform.position;
                bomb.JumpToTarget(target.transform, ownerComponent.GetComponent<Unit>());
            }
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
                    Execute();
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
