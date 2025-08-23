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
        private AddressableAsset<AudioClip> sound;
        private float checkTime;
        private int targetAttackCount;
        private int maxAttackTargetNum;
        private int attackCount;
        private bool isChecking;
        private float checkRadius;
        private int levelUpIncreaseRate;

        public StatikkShivSkill(AddressableAsset<StatikkShivLighting> prefab, AddressableAsset<AudioClip> sound,
            float checkTime, int targetAttackCount, int maxAttackTargetNum, float checkRadius, int levelUpIncreaseRate) : base()
        {
            prefab.InitializeAsync().Forget();
            sound.InitializeAsync().Forget();

            this.prefab = prefab;
            this.checkTime = checkTime;
            this.targetAttackCount = targetAttackCount;
            this.maxAttackTargetNum = maxAttackTargetNum;
            this.checkRadius = checkRadius;
            this.levelUpIncreaseRate = levelUpIncreaseRate;
            this.sound = sound;
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
            // Collider2D[] cols = Physics2D.OverlapCircleAll(ownerComponent.transform.position, checkRadius);

            // if (cols.Length == 0)
            //     return;

            // int attackTargetCount = 0;
            // foreach (var col in cols)
            // {
            //     if (col.gameObject == ownerComponent.gameObject)
            //         continue;

            //     if (col.gameObject.TryGetComponent<Unit>(out Unit unit))
            //     {
            //         attackTargetCount++;

            //         StatikkShivLighting statikkShivLighting = PoolManager.Spawn<StatikkShivLighting>(prefab);
            //         statikkShivLighting.Active(ownerComponent.GetComponent<Unit>(), 3, 10);

            //         if (attackTargetCount == maxAttackTargetNum)
            //         {
            //             break;
            //         }
            //     }
            // }
            StatikkShivLighting statikkShivLighting = PoolManager.Spawn<StatikkShivLighting>(prefab);
            statikkShivLighting.Active(ownerComponent.GetComponent<Unit>(), maxAttackTargetNum, checkRadius);
            
            // if (attackTargetCount > 0)
            // {
            //     _ = new PlaySound(sound);
            // }
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

        private async UniTask StartChecking()
        {
            isChecking = true;

            await UniTask.Delay(System.TimeSpan.FromSeconds(checkTime));

            isChecking = false;
        }

        public override void LevelUp()
        {
            base.LevelUp();

            maxAttackTargetNum += levelUpIncreaseRate;
        }
    }
}
