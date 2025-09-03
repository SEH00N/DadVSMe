using DadVSMe.Core.UI;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class UnitHealthEventListener : MonoBehaviour
    {
        [SerializeField] Unit unit = null;
        [SerializeField] AddressableAsset<DamageText> damageTextPrefab = null;

        private UnitFSMData fsmData = new UnitFSMData();

        private void Awake()
        {
            unit.OnInitializedEvent += Initialize;
        }

        private void Initialize(IEntityData data)
        {
            fsmData = unit.FSMBrain.GetAIData<UnitFSMData>();
        }

        public void OnAttack(IAttacker attacker, IAttackData attackData)
        {
            int forwardDirection = attacker.AttackerTransform.position.x > fsmData.unit.transform.position.x ? 1 : -1;
            fsmData.forwardDirection = forwardDirection;

            float currentLossyScaleX = fsmData.unit.transform.lossyScale.x;
            fsmData.unit.transform.localScale = new Vector3(fsmData.unit.transform.localScale.x * (fsmData.forwardDirection / currentLossyScaleX), 1, 1);
            fsmData.attackData = attackData;
            fsmData.hitAttribute = attacker.AttackAttribute;

            SpawnDamageText(attacker.AttackAttribute, attackData.Damage * attacker.AttackPower);
        }

        public void OnHeal(int amount)
        {
            SpawnDamageText(EAttackAttribute.Normal, -amount);
        }

        private void SpawnDamageText(EAttackAttribute attackAttribute, float damage)
        {
            if(Mathf.Abs(damage) < 0.01f)
                return;

            var handle = UIManager.CreateUIHandle<DamageTextUIHandlse, DamageTextUIHandleParameter>(out DamageTextUIHandleParameter param);
            param.target = transform;
            param.attackAttribute = attackAttribute;
            param.damageTextPrefab = damageTextPrefab;
            param.upOffset = Vector3.up;
            param.damage = damage;

            handle.Execute(param);
        }
    }
}