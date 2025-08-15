using System.Collections.Generic;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class GrabAttackAction : AttackActionBase
    {
        [System.Serializable]
        private struct GrabAttackData
        {
            public SimpleAttackData simpleAttackData;
            public string animationName;
        }

        [SerializeField] List<GrabAttackData> grabAttackDatas = new List<GrabAttackData>();

        private EntityAnimator entityAnimator = null;
        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
            entityAnimator = brain.GetComponent<EntityAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();
            entityAnimator.PlayAnimation(grabAttackDatas[fsmData.grabAttackCount % grabAttackDatas.Count].animationName);
        }

        public override void ExitState()
        {
            base.ExitState();
            fsmData.grabAttackCount++;
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            if(fsmData.grabbedEntity is not Unit grabbedUnit)
                return;

            AttackToTarget(grabbedUnit, grabAttackDatas[fsmData.grabAttackCount % grabAttackDatas.Count].simpleAttackData);
        }
    }
}
