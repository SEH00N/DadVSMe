using System;
using DadVSMe.Entities;
using DadVSMe.Players.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class BowlingSkill : UnitSkill<BowlingSkillData, BowlingSkillData.Option>
    {
        private Unit owner = null;
        private PlayerFSMData fsmData;
        // private Entity target;
        // private Collider2D checkCol;

        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            base.OnRegist(ownerComponent, skillData);
            owner = ownerComponent.GetComponent<Unit>();
            fsmData = owner.FSMBrain.GetAIData<PlayerFSMData>();
            if (fsmData == null)
                return;

            fsmData.onGrabbedEntityChanged += OnGrabbedEntityChanged;
        }

        public override void OnUnregist()
        {
            base.OnUnregist();
            fsmData.onGrabbedEntityChanged -= OnGrabbedEntityChanged;
        }

        public override void Execute()
        {

        }

        private void OnGrabbedEntityChanged(Entity target)
        {
            Unit attacker = target as Unit;
            if(attacker != null)
                attacker.FSMBrain.GetAIData<UnitFSMData>().OnBowlingEvent += (otherUnit) => HandleEnemyBowling(attacker, otherUnit);

            // if (target != null && target.TryGetComponent<FSMBrain>(out FSMBrain prevBrain))
            // {
            //     prevBrain.OnStateChangedEvent.RemoveListener(OnStateChanged);
            // }

            // this.target = target;

            // if (target.TryGetComponent<FSMBrain>(out FSMBrain brain))
            // {
            //     brain.OnStateChangedEvent.AddListener(OnStateChanged);
            // }
        }

        private void HandleEnemyBowling(Unit attacker, Unit otherUnit)
        {
            if(otherUnit == attacker || otherUnit == owner)
                return;

            DynamicAttackData attackData = new DynamicAttackData(GetData().bowlingHitAttackData);
            attackData.SetDamage(GetOption().damage);

            otherUnit.UnitHealth.Attack(attacker, attackData);
            UnitFSMData unitFSMData = ownerComponent.GetComponent<FSMBrain>().GetAIData<UnitFSMData>();
            _ = new PlayHitFeedback(attackData, unitFSMData.attackAttribute, otherUnit.transform.position, Vector3.zero, unitFSMData.forwardDirection);
        }

        // private void OnStateChanged(FSMState currentState, FSMState targetState)
        // {
        //     if (targetState.gameObject.name.Contains("Throw") || targetState.gameObject.name.Contains("Juggle"))
        //     {
        //         // checkCol = target.GetComponent<CapsuleCollider2D>();
        //         target.OnTriggerEnterEvent += OnTriggerEnter;
        //     }
        //     else
        //     {
        //         // checkCol = null;
        //         target.OnTriggerEnterEvent -= OnTriggerEnter;
        //     }
        // }

        // private void OnTriggerEnter(Collider2D col)
        // {
        //     // if (checkCol.IsTouching(col) == false)
        //     //     return;

        //     if (col.gameObject.TryGetComponent<UnitHealth>(out UnitHealth health))
        //     {
        //         DynamicAttackData attackData = new DynamicAttackData(data.bowlingHitAttackData);
        //         attackData.SetDamage(attackData.Damage + (int)(data.levelUpIncreaseRate * level));

        //         health.Attack(target as Unit, attackData);
        //         UnitFSMData unitFSMData = ownerComponent.GetComponent<FSMBrain>().GetAIData<UnitFSMData>();
        //         _ = new PlayHitFeedback(attackData, unitFSMData.attackAttribute, health.transform.position, Vector3.zero, unitFSMData.forwardDirection);
        //     }
        // }
    }
}
