using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class NinjaJumpAction : FSMAction
    {
        private const string NINJA_SPIN_ANIM_NAME = "NinjaSpin";
        [SerializeField] FSMState ninjaAttackState;
        [SerializeField] EntityAnimationEventListener animationEventListener = null;
        [SerializeField] AddressableAsset<PoolableAnimationEffect> jumpupDustFX;

        [Header("Value")]
        [SerializeField] Vector2 jumpVelocity;

        private UnitFSMData unitFSMData = null;
        private Rigidbody2D unitRigidbody = null;
        private EntityAnimator entityAnimator = null;

        private bool _onSpinning;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            unitFSMData = brain.GetAIData<UnitFSMData>();
            unitRigidbody = brain.GetComponent<Rigidbody2D>();
            entityAnimator = brain.GetComponent<EntityAnimator>();

            jumpupDustFX.InitializeAsync().Forget();
        }

        public override void EnterState()
        {
            base.EnterState();

            unitFSMData.unit.SetFloat(true);
            unitRigidbody.linearVelocity = jumpVelocity;
            unitRigidbody.gravityScale = 2;

            _onSpinning = false;

            var fx = PoolManager.Spawn<PoolableAnimationEffect>(jumpupDustFX.Key, GameInstance.GameCycle.transform);
            fx.transform.position = brain.transform.position;
            fx.Play();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(unitRigidbody.linearVelocity.y <= 0 && _onSpinning == false)
            {
                unitRigidbody.gravityScale = 0;
                _onSpinning = true;

                animationEventListener.AddEventListener(EEntityAnimationEventType.End, HandleChangeAttackState);
                entityAnimator.PlayAnimation(NINJA_SPIN_ANIM_NAME);
            }
        }

        private void HandleChangeAttackState(EntityAnimationEventData eventData)
        {
            brain.ChangeState(ninjaAttackState);
        }

        public override void ExitState()
        {
            animationEventListener.RemoveEventListener(EEntityAnimationEventType.End, HandleChangeAttackState);
            unitFSMData.unit.transform.rotation = Quaternion.identity;
        }
    }
}
