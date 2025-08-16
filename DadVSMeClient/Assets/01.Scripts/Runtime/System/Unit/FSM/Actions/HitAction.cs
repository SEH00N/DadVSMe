using Cysharp.Threading.Tasks;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class HitAction : FSMAction
    {
        [SerializeField] AddressableAsset<ParticleSystem> hitEffect = null;
        [SerializeField] AddressableAsset<AudioClip> hitSound = null;
        
        protected UnitFSMData unitFSMData = null;
        protected IAttackData attackData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
            hitEffect?.InitializeAsync().Forget();
            hitSound?.InitializeAsync().Forget();
        }

        public override void EnterState()
        {
            base.EnterState();
            attackData = unitFSMData.attackData;
            unitFSMData.attackData = null;

            if(attackData.Damage >= 0 && hitSound != null)
                AudioManager.Instance.PlaySFX(hitSound);
        }
    }
}