using System;
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

        public AttackBlastSkill(AddressableAsset<AttackBlast> prefab)
        {
            this.prefab = prefab;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.AddListener(OnOwnerStatChanged);
        }

        public override void Execute()
        {
            AttackBlast attackBlast = PoolManager.Spawn<AttackBlast>(prefab);
            
            attackBlast.SetInstigator(ownerComponent.gameObject);
            Transform ownerTrm = ownerComponent.transform;
            attackBlast.transform.position = ownerTrm.position + (new Vector3(3f, 0f) * Math.Sign(ownerTrm.localScale.x));
            //attackBlast.Lunch(ownerTrm.right * Math.Sign(ownerTrm.localScale.x));
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