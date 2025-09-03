using H00N.AI.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class UnitDespawnAction : FSMAction
    {
        protected EntityAnimator entityAnimator = null;
        protected UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            entityAnimator = brain.GetComponent<EntityAnimator>();
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEndEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEndEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEndEvent);
        }

        private void HandleAnimationEndEvent(EntityAnimationEventData eventData)
        {
            PoolManager.Despawn(unitFSMData.unit);
        }
    }
}
