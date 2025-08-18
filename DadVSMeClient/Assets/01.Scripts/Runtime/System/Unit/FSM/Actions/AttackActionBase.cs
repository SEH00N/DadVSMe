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
        [SerializeField] Vector2 attackOffset = Vector2.zero;
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
            
            foreach (EAttackAttribute attackAttribute in Enum.GetValues(typeof(EAttackAttribute)))
            {
                if (attackData.GetFeedbackData(attackAttribute) == null)
                    continue;

                attackData.GetFeedbackData(attackAttribute).hitEffects.ForEach(effect => effect.InitializeAsync().Forget());
                attackData.GetFeedbackData(attackAttribute).attackSounds.ForEach(sound => sound.InitializeAsync().Forget());
                attackData.GetFeedbackData(attackAttribute).hitSounds.ForEach(sound => sound.InitializeAsync().Forget());
            }
        }

        public override void EnterState()
        {
            base.EnterState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);

            _ = new PlaySound(attackData.GetFeedbackData(EAttackAttribute.Normal).attackSounds);
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
            attackData.attackAttribute = unitFSMData.attackAttribute;
            target.UnitHealth.Attack(unitFSMData.unit, attackData);
            unitFSMData.unit.onAttackTargetEvent?.Invoke(target, attackData);

            if(playEffect)
            {
                Vector3 offset = new Vector3(attackOffset.x * unitFSMData.forwardDirection, attackOffset.y, 0f);
                attackData.GetFeedbackData(EAttackAttribute.Normal).hitEffects.
                    ForEach(effect => _ = new PlayEffect(effect, target.transform.position + offset, unitFSMData.forwardDirection));
                _ = new PlaySound(attackData.GetFeedbackData(EAttackAttribute.Normal).hitSounds);
            }
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