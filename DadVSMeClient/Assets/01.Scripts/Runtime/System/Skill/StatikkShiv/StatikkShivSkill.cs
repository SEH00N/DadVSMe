using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class StatikkShivSkill : UnitSkill<StatikkShivSkillData, StatikkShivSkillData.Option>
    {
        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            base.OnRegist(ownerComponent, skillData);

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.RemoveListener(OnAttackTarget);
        }

        private void OnAttackTarget(Unit target, IAttackData data)
        {
            if(Random.value > GetOption().executeChance)
                return;

            Execute();
        }

        public async override void Execute()
        {
            StatikkShivSkillData data = GetData();
            StatikkShivSkillData.Option option = GetOption();

            AddressableAsset<StatikkShivLighting> prefab = data.prefab;
            AttackDataBase attackData = data.attackData;
            float checkRadius = data.checkRadius;
            int additiveDamage = option.additiveDamage;

            await prefab.InitializeAsync();

            StatikkShivLighting statikkShivLighting = PoolManager.Spawn<StatikkShivLighting>(prefab, GameInstance.GameCycle.transform);
            DynamicAttackData dynamicAttackData = new DynamicAttackData(attackData);
            dynamicAttackData.SetDamage(attackData.Damage + additiveDamage);
            statikkShivLighting.Active(ownerComponent.GetComponent<Unit>(), checkRadius, dynamicAttackData, dynamicAttackData);
        }
    }
}
