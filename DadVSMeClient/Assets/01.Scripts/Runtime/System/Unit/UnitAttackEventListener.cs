using DadVSMe.Core.UI;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class UnitAttackEventListener : MonoBehaviour
    {
        [SerializeField] Unit unit = null;

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

            SpawnDamageText(attacker, attackData, attackData as IAttackFeedbackDataContainer);
        }

        private void SpawnDamageText(IAttacker attacker, IAttackData attackData, IAttackFeedbackDataContainer feedbackData)
        {
            if(feedbackData == null)
                return;

            var handle = UIManager.CreateUIHandle<DamageTextUIHandlse, DamageTextUIHandleParameter>(out DamageTextUIHandleParameter param);
            param.target = transform;
            param.attackAttribute = attacker.AttackAttribute;
            param.feedbackData = feedbackData;
            param.upOffset = Vector3.up;
            param.damage = attackData.Damage * attacker.AttackPower;

            handle.Execute(param);
        }
    }
}