using System;
using DadVSMe.Core.Cam;
using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class SunchipsFrenzyAction : AttackActionBase
    {
        SunchipsEnemyFSMData fsmData;
        Unit target;
        public Transform targetPosition;
        public Vector3 hitUpOffset;
        public Color filterColor;
        public float filterVisibleTime;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<SunchipsEnemyFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            target = fsmData.frenzyTarget;
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, FirstAttack);

            BackgroundFilterCameraHandle handle =
                CameraManager.CreateCameraHandle<BackgroundFilterCameraHandle, BackgroundFilterCameraHandleParameter>
                (out BackgroundFilterCameraHandleParameter param);

            param.time = filterVisibleTime;
            param.color = filterColor;
            handle.ExecuteAsync(param);
        }

        public override void ExitState()
        {
            base.ExitState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, AfterAttack);
        }

        private void FirstAttack(EntityAnimationEventData data)
        {
            target.transform.position = targetPosition.position;

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, FirstAttack);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, AfterAttack);
        }

        private void AfterAttack(EntityAnimationEventData data)
        {
            target.transform.position += hitUpOffset;

            AttackToTarget(fsmData.frenzyTarget, attackData);
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            
        }
    }
}
