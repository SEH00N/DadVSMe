using System;
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
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 scale = Vector3.one;
        private ParticleSystem particle;

        private Unit owner;

        public override async void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            await powerUpParticle.InitializeAsync();
            owner = brain.GetComponent<Unit>();
        }

        public override void EnterState()
        {
            base.EnterState();

            particle = PoolManager.Spawn<ParticleSystem>(powerUpParticle);
            particle.transform.SetParent(brain.transform);
            particle.transform.localPosition = positionOffset;
            particle.transform.localScale = scale;

            owner.onEndAngerEvent.AddListener(OnEndAnger);
        }

        private void OnEndAnger()
        {
            PoolManager.Despawn(particle.GetComponent<PoolReference>());
            owner.onEndAngerEvent.RemoveListener(OnEndAnger);
        }
    }
}
