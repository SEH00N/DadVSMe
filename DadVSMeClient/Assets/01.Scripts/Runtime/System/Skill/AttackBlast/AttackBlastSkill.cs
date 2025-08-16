using System;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using DadVSMe.Players.FSM;
using H00N.AI.FSM;
using H00N.Resources;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AttackBlastSkill : UnitSkill
    {
        private AddressableAsset<AttackBlast> prefab = null;

        private Vector3 spawnOffset;

        public AttackBlastSkill(AddressableAsset<AttackBlast> prefab)
        {
            this.prefab = prefab;
            spawnOffset = new Vector2(2.5f, 0f);
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.AddListener(OnOwnerStatChanged);
        }

        public override void Execute()
        {
            AttackBlast attackBlast = PoolManager.Spawn<AttackBlast>(prefab);
            
            attackBlast.SetInstigator(ownerComponent.gameObject.GetComponent<Unit>());

            Transform ownerTrm = ownerComponent.transform;
            attackBlast.transform.position = ownerTrm.position + (spawnOffset * Math.Sign(ownerTrm.localScale.x));
            Vector3 scale = attackBlast.transform.localScale;
            scale.x *= Math.Sign(ownerTrm.localScale.x);
            attackBlast.transform.localScale = scale;

            attackBlast.Launch(ownerTrm.right * Math.Sign(ownerTrm.localScale.x));
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.RemoveListener(OnOwnerStatChanged);
        }

        private void OnOwnerStatChanged(FSMState current, FSMState target)
        {
            if (target.TryGetComponent<AttackActionBase>(out AttackActionBase attack))
            {
                Execute();
            }
        }   
    }
}