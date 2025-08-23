using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class BounceAction : FSMAction
    {
        private const float HARD_BOUNCE_FORCE_THRESHOLD = 30f;

        [SerializeField] FSMState lieState = null;
        [SerializeField] float bounciness = 0.8f;
        [SerializeField] float minVelocity = 0.1f;

        [Space(10f)]
        [SerializeField] List<string> bounceAnimations = new List<string>();

        [Space(10f)]
        [SerializeField] Vector2 bounceEffectOffset = Vector2.zero;
        [SerializeField] AddressableAsset<PoolableEffect> smallBounceEffect = null;
        [SerializeField] AddressableAsset<PoolableEffect> hardBounceEffect = null;
        [SerializeField] List<AddressableAsset<AudioClip>> bounceSounds = new List<AddressableAsset<AudioClip>>();

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

            smallBounceEffect.InitializeAsync().Forget();
            hardBounceEffect.InitializeAsync().Forget();
            bounceSounds.ForEach(sound => sound.InitializeAsync().Forget());
        }

        public override void EnterState()
        {
            base.EnterState();

            Vector2 forceDirection = Vector2.Reflect(unitFSMData.collisionData.force.normalized, unitFSMData.collisionData.normal);
            float collisionForce = unitFSMData.collisionData.force.magnitude;
            float bounceForce = collisionForce * bounciness;
            unitRigidbody.linearVelocity = forceDirection * bounceForce;

            entityAnimator.PlayAnimation(bounceAnimations[currentBounceAnimationIndex]);
            currentBounceAnimationIndex = (currentBounceAnimationIndex + 1) % bounceAnimations.Count;

            Vector3 offset = new Vector3(bounceEffectOffset.x * unitFSMData.forwardDirection, bounceEffectOffset.y, 0f);
            _ = new PlayEffect(collisionForce > HARD_BOUNCE_FORCE_THRESHOLD ? hardBounceEffect : smallBounceEffect, brain.transform.position + offset, -unitFSMData.forwardDirection);
            _ = new PlaySound(bounceSounds);
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