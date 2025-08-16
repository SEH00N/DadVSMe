using System.Collections.Generic;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class BounceAction : FSMAction
    {
        [SerializeField] FSMState lieState = null;
        [SerializeField] float bounciness = 0.8f;
        [SerializeField] float minVelocity = 0.1f;

        [Space(10f)]
        [SerializeField] List<string> bounceAnimations = new List<string>();

        private UnitFSMData unitFSMData = null;
        private Rigidbody2D unitRigidbody = null;
        private EntityAnimator entityAnimator = null;
        
        private int currentBounceAnimationIndex = 0;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
            unitRigidbody = brain.GetComponent<Rigidbody2D>();
            entityAnimator = brain.GetComponent<EntityAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();

            Vector2 forceDirection = Vector2.Reflect(unitFSMData.collisionData.force.normalized, unitFSMData.collisionData.normal);
            float forceMagnitude = unitFSMData.collisionData.force.magnitude * bounciness;
            unitRigidbody.linearVelocity = forceDirection * forceMagnitude;

            string animName = unitFSMData.hitAttribute == EAttackAttribute.Normal ?
                bounceAnimations[currentBounceAnimationIndex] :
                $"{bounceAnimations[currentBounceAnimationIndex]}_{unitFSMData.hitAttribute}";
            entityAnimator.PlayAnimation(animName);
            currentBounceAnimationIndex = (currentBounceAnimationIndex + 1) % bounceAnimations.Count;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            // if the bounce is not due to a collision, the bounce is checked based on the last ground position.
            if(brain.transform.position.y >= unitFSMData.groundPositionY || unitRigidbody.linearVelocity.y > 0)
                return;

            if(unitRigidbody.linearVelocity.magnitude < minVelocity)
            {
                // stop bouncing
                brain.transform.position = new Vector3(brain.transform.position.x, unitFSMData.groundPositionY, brain.transform.position.z);
                unitRigidbody.linearVelocity = Vector2.zero;
                unitFSMData.unit.SetFloat(false);
                brain.ChangeState(lieState);
            }
            else
            {
                // re-bounce
                unitFSMData.collisionData = new UnitCollisionData(unitRigidbody.linearVelocity, Vector2.up, new Vector2(brain.transform.position.x, unitFSMData.groundPositionY));
                brain.ChangeState(state); 
            }
        }
    }
}