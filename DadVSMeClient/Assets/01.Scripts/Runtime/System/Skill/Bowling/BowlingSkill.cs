using System;
using DadVSMe.Entities;
using DadVSMe.Players.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class BowlingSkill : UnitSkill
    {
        private BowlingSkillData data;
        private PlayerFSMData fsmData;
        Entity target;
        Collider2D checkCol;

        public BowlingSkill(BowlingSkillData data)
        {
            this.data = data;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);
            fsmData = ownerComponent.GetComponent<FSMBrain>().GetAIData<PlayerFSMData>();
            if (fsmData == null)
                return;

            fsmData.onGrabbedEntityChanged += OnGrabbedEntityChanged;
        }

        public override void Execute()
        {

        }

        private void OnGrabbedEntityChanged(Entity target)
        {
            if (target != null && target.TryGetComponent<FSMBrain>(out FSMBrain prevBrain))
            {
                prevBrain.OnStateChangedEvent.RemoveListener(OnStateChanged);
            }

            this.target = target;

            if (target.TryGetComponent<FSMBrain>(out FSMBrain brain))
            {
                brain.OnStateChangedEvent.AddListener(OnStateChanged);
            }
        }

        private void OnStateChanged(FSMState currentState, FSMState targetState)
        {
            if (targetState.gameObject.name.Contains("Throw") || targetState.gameObject.name.Contains("Juggle"))
            {
                checkCol = target.GetComponent<CapsuleCollider2D>();
                target.onTriggerEnter.AddListener(OnTriggerEnter);
            }
            else
            {
                checkCol = null;
                target.onTriggerEnter.RemoveListener(OnTriggerEnter);
            }
        }

        private void OnTriggerEnter(Collider2D col)
        {
            if (checkCol.IsTouching(col) == false)
                return;

            if (col.gameObject.TryGetComponent<UnitHealth>(out UnitHealth health))
            {
                DynamicAttackData attackData = new DynamicAttackData(data.bowlingHitAttackData);
                attackData.SetDamage(attackData.Damage + (int)(data.levelUpIncreaseRate * level));

                health.Attack(target as Unit, attackData);
                UnitFSMData unitFSMData = ownerComponent.GetComponent<FSMBrain>().GetAIData<UnitFSMData>();
                _ = new PlayAttackFeedback(attackData, unitFSMData.attackAttribute, health.transform.position, Vector3.zero, unitFSMData.forwardDirection);
            }
        }
    }
}
