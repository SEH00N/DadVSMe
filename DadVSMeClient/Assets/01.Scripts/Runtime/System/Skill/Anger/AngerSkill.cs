using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class AngerSkill : UnitSkill
    {
        private float maxAngerGauge;
        private float currentAngerGauge;
        private float angerTime;
        private bool isAnger;

        public AngerSkill(float maxAngerGauge = 100, float angerTime = 10f) : base()
        {
            this.maxAngerGauge = maxAngerGauge;
            this.angerTime = angerTime;
            currentAngerGauge = 0f;
            isAnger = false;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            ownerComponent.GetComponent<Unit>().onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        public override async void Execute()
        {
            await ActiveAnger();
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            ownerComponent.GetComponent<Unit>().onAttackTargetEvent.RemoveListener(OnAttackTarget);
        }

        private void OnAttackTarget(Unit target, IAttackData attackData)
        {
            if (isAnger)
                return;

            currentAngerGauge = Mathf.Min(currentAngerGauge + 5, maxAngerGauge);
            
            if (currentAngerGauge >= maxAngerGauge)
            {
                Execute();
            }
        }

        private async UniTask ActiveAnger()
        {
            isAnger = true;

            ownerComponent.GetComponent<EntityAnimator>().PlayAnimation("PowerUp");

            await UniTask.Delay(System.TimeSpan.FromSeconds(angerTime));
            
            isAnger = false;
            currentAngerGauge = 0f;
        }
    }
}
