using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class ThrowAttackAction : AttackActionBase
    {
        private class ThrowData : IAttackData
        {
            private EAttackFeedback attackFeedback;
            public EAttackFeedback AttackFeedback => attackFeedback;
            public int Damage => 0;

            public ThrowData(EAttackFeedback attackFeedback)
            {
                this.attackFeedback = attackFeedback;
            }
        }

        [Space(10f)]
        [SerializeField] EAttackFeedback throwPreFeedback;
        [SerializeField] AttackDataBase attackData = null;

        [Space(10f)]
        [SerializeField] Collider2D defaultSortingOrderResolverCollider = null;
        [SerializeField] Collider2D grabbedSortingOrderResolverCollider = null;

        private PlayerFSMData fsmData = null;

        protected override IAttackFeedbackDataContainer FeedbackDataContainer => attackData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            fsmData.grabParent.localPosition = attackData.AttackFeedback == EAttackFeedback.Throw1 ? fsmData.throw1Position.localPosition : fsmData.throw2Position.localPosition;
            AttackToTarget(fsmData.grabbedEntity as Unit, new ThrowData(throwPreFeedback), false);
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            fsmData.grabParent.localPosition = fsmData.grabPosition.localPosition;

            if(fsmData.grabbedEntity == null)
                return;

            Entity grabbedEntity = fsmData.grabbedEntity;
            fsmData.grabbedEntity = null;

            grabbedEntity.transform.SetParent(null);
            (grabbedEntity as IGrabbable).Release(unitFSMData.unit);
            (grabbedEntity as Unit).FSMBrain.GetAIData<UnitFSMData>().groundPositionY = unitFSMData.groundPositionY;

            defaultSortingOrderResolverCollider.enabled = true;
            grabbedSortingOrderResolverCollider.enabled = false;

            AttackToTarget(grabbedEntity as Unit, attackData);
        }
    }
}
