using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AttackBlastSkill : UnitSkill<AttackBlastSkillData, AttackBlastSkillData.Option>
    {
        private const float SPAWN_RANDOMNESS = 0.5f;
        private static readonly Vector2 SpawnOffset = new Vector2(1.5f, 2f);

        // private float attackBlastLifeTime;

        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            base.OnRegist(ownerComponent, skillData);

            GetData().prefab.InitializeAsync().Forget();
            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.AddListener(OnOwnerStatChanged);
        }

        public override void Execute()
        {
            AddressableAsset<AttackBlast> prefab = GetData().prefab;
            float attackBlastLifeTime = GetOption().attackBlastLifeTime;

            AttackBlast attackBlast = PoolManager.Spawn<AttackBlast>(prefab, GameInstance.GameCycle.transform);
            attackBlast.Initialize(GetOption().additiveDamage);
            attackBlast.SetInstigator(ownerComponent.gameObject.GetComponent<Unit>());

            Transform ownerTrm = ownerComponent.transform;
            attackBlast.transform.position = ownerTrm.position + new Vector3(SpawnOffset.x * Math.Sign(ownerTrm.localScale.x), SpawnOffset.y, 0f) + (Vector3)(UnityEngine.Random.insideUnitCircle * SPAWN_RANDOMNESS);
            Vector3 scale = attackBlast.transform.localScale;
            scale.x *= Math.Sign(ownerTrm.localScale.x);
            attackBlast.transform.localScale = scale;

            attackBlast.Launch(ownerTrm.right * Math.Sign(ownerTrm.localScale.x), attackBlastLifeTime);
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
                if(target.gameObject.name.Contains("Punch1_"))
                    Execute();
            }
        }
    }
}