using System;
using Cysharp.Threading.Tasks;
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
        private const float SPAWN_RANDOMNESS = 0.5f;
        private static readonly Vector2 SpawnOffset = new Vector2(1.5f, 2f);

        private AddressableAsset<AttackBlast> prefab = null;
        private float attackBlastLifeTime;
        private float levelUpIncreaseRate;

        public AttackBlastSkill(AddressableAsset<AttackBlast> prefab, float attackBlastLifeTime, float levelUpIncreaseRate)
        {
            prefab.InitializeAsync().Forget();

            this.prefab = prefab;
            this.attackBlastLifeTime = attackBlastLifeTime;
            this.levelUpIncreaseRate = levelUpIncreaseRate;
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

        public override void LevelUp()
        {
            base.LevelUp();

            attackBlastLifeTime += levelUpIncreaseRate;
        }
    }
}