using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class PowerUpAction : FSMAction
    {
        [SerializeField] private AddressableAsset<ParticleSystem> powerUpParticle = null;
        [SerializeField] private AddressableAsset<AudioClip> sound = null;
        [SerializeField] private Vector3 positionOffset;
        private ParticleSystem particle;

        private Unit owner;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            powerUpParticle.InitializeAsync().Forget();
            sound.InitializeAsync().Forget();
            owner = brain.GetComponent<Unit>();
        }

        public override void EnterState()
        {
            base.EnterState();

            particle = PoolManager.Spawn<ParticleSystem>(powerUpParticle, GameInstance.GameCycle.transform);
            particle.transform.SetParent(brain.transform);
            particle.transform.localPosition = positionOffset;
            particle.transform.localScale = Vector3.one;
            _ = new PlaySound(sound);

            owner.onEndAngerEvent.AddListener(OnEndAnger);
        }

        private void OnEndAnger()
        {
            PoolManager.Despawn(particle.GetComponent<PoolReference>());
            owner.onEndAngerEvent.RemoveListener(OnEndAnger);
        }
    }
}
