using DadVSMe.Core.Cam;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using DG.Tweening;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsFrenzyAction : AttackActionBase
    {
        public struct NonJuggleAttackData : IAttackData, IJuggleAttackData
        {
            public readonly int Damage => 0;
            public readonly EAttackFeedback AttackFeedback => EAttackFeedback.Juggle;
            public readonly float JuggleForce => 1f;
            public readonly Vector2 JuggleDirection => Vector2.up;
        }

        public struct NonFrenzyAttackData : IAttackData
        {
            public readonly int Damage => 0;
            public readonly EAttackFeedback AttackFeedback => EAttackFeedback.Frenzy;
        }

        private static readonly NonJuggleAttackData NON_JUGGLE_ATTACK_DATA = new NonJuggleAttackData();
        private static readonly NonFrenzyAttackData NON_FRENZY_ATTACK_DATA = new NonFrenzyAttackData();
        private const float X_OFFSET = 1f;
        protected override IAttackFeedbackDataContainer FeedbackDataContainer => attackData;

        [SerializeField] AttackDataBase attackData;
        [SerializeField] Transform targetPosition;
        [SerializeField] Vector3 hitUpOffset;
        [SerializeField] Color filterColor;
        [SerializeField] float filterVisibleTime;

        private SunchipsEnemyFSMData fsmData;
        private Unit target;
        private Rigidbody2D targetRigidbody;

        private int attackCount = 0;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<SunchipsEnemyFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            target = fsmData.frenzyTarget;
            targetRigidbody = target.GetComponent<Rigidbody2D>();
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, FirstAttack);

            BackgroundFilterCameraHandle handle =
                CameraManager.CreateCameraHandle<BackgroundFilterCameraHandle, BackgroundFilterCameraHandleParameter>
                (out BackgroundFilterCameraHandleParameter param);

            param.time = filterVisibleTime;
            param.color = filterColor;
            handle.ExecuteAsync(param);

            attackCount = 0;
        }

        public override void ExitState()
        {
            base.ExitState();

            AttackToTarget(target, NON_JUGGLE_ATTACK_DATA);
            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, AfterAttack);
        }

        private void FirstAttack(EntityAnimationEventData data)
        {
            target.SetFloat(true);
            targetRigidbody.gravityScale = 0f;
            target.transform.position = targetPosition.position + GetOffset();
            AttackToTarget(target, NON_FRENZY_ATTACK_DATA);

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, FirstAttack);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, AfterAttack);
        }

        private void AfterAttack(EntityAnimationEventData data)
        {
            attackCount++;
            target.transform.DOKill();

            Vector3 targetPosition = target.transform.position + hitUpOffset + (GetOffset() * 2f);
            target.transform.DOMove(targetPosition, 0.1f).OnKill(() => {
                target.transform.position = targetPosition;
            });

            // target.transform.position += hitUpOffset + (GetOffset() * 2f);

            AttackToTarget(target, attackData);
            // targetRigidbody.gravityScale = 0f;s
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            
        }

        private Vector3 GetOffset()
        {
            float sign = attackCount % 2 == 0 ? 1 : -1;
            return Vector3.right * (X_OFFSET * sign);
        }
    }
}
