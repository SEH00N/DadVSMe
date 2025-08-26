using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class PlayerBossClearDirectingBounceAction : FSMAction
    {
        private const float HARD_BOUNCE_FORCE_THRESHOLD = 20f;

        [SerializeField] float cameraShakeDuration = 0.1f;
        [SerializeField] float cameraShakeAmplitude = 8f;
        [SerializeField] float cameraShakeFrequency = 7f;

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            float collisionForce = unitFSMData.collisionData.force.magnitude;
            if(collisionForce < HARD_BOUNCE_FORCE_THRESHOLD)
                return;

            _ = new ShakeCamera(GameInstance.GameCycle.MainCinemachineCamera, cameraShakeDuration, cameraShakeAmplitude, cameraShakeFrequency);
        }
    }
}