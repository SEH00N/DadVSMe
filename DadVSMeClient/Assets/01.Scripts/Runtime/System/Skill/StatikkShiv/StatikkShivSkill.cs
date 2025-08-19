using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class StatikkShivSkill : UnitSkill
    {
        private AddressableAsset<StatikkShivLighting> prefab;
        private float checkTime;
        private int targetAttackCount;
        private int maxAttackTargetNum;
        private int attackCount;
        private bool isChecking;
        private float damage;

        public StatikkShivSkill(AddressableAsset<StatikkShivLighting> prefab,
            float checkTime = 8f, int targetAttackCount = 3, int maxAttackTargetNum = 5, float damage = 5f) : base()
        {
            prefab.InitializeAsync().Forget();

            this.prefab = prefab;
            this.checkTime = checkTime;
            this.targetAttackCount = targetAttackCount;
            this.maxAttackTargetNum = maxAttackTargetNum;
            this.damage = damage;
            attackCount = 0;
            isChecking = false;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.AddListener(OnOwnerStatChanged);
        }

        public override void Execute()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(ownerComponent.transform.position, 10f);

            if (cols.Length == 0)
                return;

            int attackTargetCount = 0;
            foreach (var col in cols)
            {
                if (col.gameObject == ownerComponent.gameObject)
                    continue;

                if (col.gameObject.TryGetComponent<Unit>(out Unit unit))
                {
                    attackTargetCount++;

                    StatikkShivLighting statikkShivLighting = PoolManager.Spawn<StatikkShivLighting>(prefab);
                    statikkShivLighting.Active(ownerComponent.GetComponent<Unit>(), unit);

                    if (attackTargetCount == maxAttackTargetNum)
                    {
                        break;
                    }
                }
            }
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.RemoveListener(OnOwnerStatChanged);
        }

        private async void OnOwnerStatChanged(FSMState current, FSMState target)
        {
            if (target.TryGetComponent<AttackActionBase>(out AttackActionBase attack))
            {
                if (isChecking == false)
                {
                    attackCount = 1;
                    await StartChecking();
                }
                else
                {
                    attackCount++;
                    if (attackCount == targetAttackCount)
                    {
                        Execute();
                    }
                }
            }

        }
        async UniTask StartChecking()
        {
            isChecking = true;

            await UniTask.Delay(System.TimeSpan.FromSeconds(checkTime));

            isChecking = false;
        }
    }
}
