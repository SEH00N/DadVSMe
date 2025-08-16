using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class BumpAttackAction : FSMAction
    {
        [SerializeField] FSMState onGoingState = null;

        [Space(10f)]
        [SerializeField] AttackDataBase attackData = null;
        [SerializeField] float attackRange = 3f;

        [Space(10f)]
        [SerializeField] AddressableAsset<ParticleSystem> attackEffect = new AddressableAsset<ParticleSystem>();
        [SerializeField] List<AddressableAsset<AudioClip>> hitSounds = new List<AddressableAsset<AudioClip>>();

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();

            if(string.IsNullOrEmpty(attackEffect.Key) == false)
                attackEffect.InitializeAsync().Forget();

            hitSounds.ForEach(sound => sound.InitializeAsync().Forget());
        }

        public override void EnterState()
        {
            base.EnterState();
            
            int forwardDirection = unitFSMData.forwardDirection;
            unitFSMData.enemies.ForEach(enemy => {
                float targetDirection = enemy.transform.position.x - transform.position.x;
                if(Mathf.Sign(targetDirection) != Mathf.Sign(forwardDirection))
                    return;

                if(Mathf.Abs(targetDirection) > attackRange)
                    return;

                if(enemy.FSMBrain.GetAIData<UnitFSMData>().isFloat)
                    return;

                if(enemy.FSMBrain.GetAIData<UnitFSMData>().isLie)
                    return;

                enemy.UnitHealth.Attack(unitFSMData.unit, attackData);

                _ = new PlayEffect(attackEffect, enemy.transform.position);
                _ = new PlaySound(hitSounds);
            });

            brain.ChangeState(onGoingState);
        }
    }
}
