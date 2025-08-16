using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public abstract class AttackActionBase : FSMAction
    {
        [SerializeField] AddressableAsset<PoolableEffect> attackEffect = new AddressableAsset<PoolableEffect>();
        [SerializeField] AddressableAsset<PoolableEffect> hitEffect = new AddressableAsset<PoolableEffect>();
        [SerializeField] List<AddressableAsset<AudioClip>> attackSounds = new List<AddressableAsset<AudioClip>>();
        [SerializeField] List<AddressableAsset<AudioClip>> hitSounds = new List<AddressableAsset<AudioClip>>();

        protected EntityAnimator entityAnimator = null;
        protected UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            entityAnimator = brain.GetComponent<EntityAnimator>();
            unitFSMData = brain.GetAIData<UnitFSMData>();

            if(string.IsNullOrEmpty(attackEffect.Key) == false)
                attackEffect.InitializeAsync().Forget();

            if(string.IsNullOrEmpty(hitEffect.Key) == false)
                hitEffect.InitializeAsync().Forget();

            attackSounds.ForEach(sound => sound.InitializeAsync().Forget());
            hitSounds.ForEach(sound => sound.InitializeAsync().Forget());
        }

        public override void EnterState()
        {
            base.EnterState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);

            _ = new PlaySound(attackSounds);
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

        protected void AttackToTarget(Unit target, AttackDataBase attackData)
        {
            target.UnitHealth.Attack(unitFSMData.unit, attackData);
            brain.GetComponent<Unit>().onAttackTargetEvent?.Invoke(target, attackData);

            _ = new PlayEffect(attackEffect, target.transform.position);
            _ = new PlayEffect(hitEffect, target.transform.position);
            _ = new PlaySound(hitSounds);
        }
    }
}