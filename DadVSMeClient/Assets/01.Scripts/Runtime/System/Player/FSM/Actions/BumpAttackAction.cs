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
        [SerializeField] UnitStateChecker unitStateChecker = null;

        [Space(10f)]
        [SerializeField] AddressableAsset<PoolableEffect> attackEffect = new AddressableAsset<PoolableEffect>();
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
            
            unitStateChecker.Check(unitFSMData.unit, unitFSMData.enemies, enemy => {
                enemy.UnitHealth.Attack(unitFSMData.unit, attackData);
                _ = new PlayEffect(attackEffect, enemy.transform.position, unitFSMData.forwardDirection);
                _ = new PlaySound(hitSounds);
            });

            brain.ChangeState(onGoingState);
        }
    }
}
