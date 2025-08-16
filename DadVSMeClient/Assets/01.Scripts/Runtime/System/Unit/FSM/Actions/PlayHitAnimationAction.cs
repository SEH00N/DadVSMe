using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class PlayHitAnimationAction : FSMAction
    {
        [SerializeField] string targetAnimationName;

        protected UnitFSMData unitFSMData = null;

        private EntityAnimator anim;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
            anim = brain.GetComponent<EntityAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();
            string targetAnim = unitFSMData.hitAttribute == EAttackAttribute.Normal ?
                targetAnimationName : $"{targetAnimationName}_{unitFSMData.hitAttribute}";

            anim.PlayAnimation(targetAnim);
        }
    }
}
