using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using Mono.Cecil.Cil;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public abstract class AttackActionBase : FSMAction
    {
        [SerializeField] protected Vector2 attackOffset = Vector2.zero;
        [SerializeField] List<AddressableAsset<PoolableEffect>> hitEffects = new List<AddressableAsset<PoolableEffect>>();
        [SerializeField] List<AddressableAsset<AudioClip>> attackSounds = new List<AddressableAsset<AudioClip>>();
        [SerializeField] List<AddressableAsset<AudioClip>> hitSounds = new List<AddressableAsset<AudioClip>>();

        [SerializeField] protected AttackDataBase attackData = null;

        protected EntityAnimator entityAnimator = null;
        protected UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            entityAnimator = brain.GetComponent<EntityAnimator>();
            unitFSMData = brain.GetAIData<UnitFSMData>();

            _ = new InitializeAttackFeedback(attackData);
        }

        public override void EnterState()
        {
            base.EnterState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);

            _ = new PlayAttackSound(attackData, unitFSMData.attackAttribute);
        }

        public override void ExitState()
        {
            base.ExitState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
        }

        protected abstract void OnAttack(EntityAnimationEventData eventData);
        private void HandleAnimationTriggerEvent(EntityAnimationEventData eventData)
        {
            OnAttack(eventData);
        }
        
        protected void AttackToTarget(Unit target, AttackDataBase attackData, bool playEffect = true)
        {
            target.UnitHealth.Attack(unitFSMData.unit, attackData);
            unitFSMData.unit.onAttackTargetEvent?.Invoke(target, attackData);

            if (playEffect)
                _ = new PlayAttackFeedback(attackData, unitFSMData.attackAttribute, target.transform.position, attackOffset, unitFSMData.forwardDirection);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(UnityEditor.Selection.activeObject != gameObject)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)attackOffset, 0.25f);
        }
        #endif
    }
}