using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class UnitAttackEventListener : MonoBehaviour
    {
        [SerializeField] FSMBrain fsmBrain = null;

        private UnitFSMData fsmData = new UnitFSMData();

        public void Initialize()
        {
            fsmData = fsmBrain.GetAIData<UnitFSMData>();
        }

        public void OnAttack(Unit attacker, IAttackData attackData)
        {
            int forwardDirection = attacker.transform.position.x > fsmData.unit.transform.position.x ? 1 : -1;
            fsmData.forwardDirection = forwardDirection;

            float currentLossyScaleX = fsmData.unit.transform.lossyScale.x;
            fsmData.unit.transform.localScale = new Vector3(fsmData.unit.transform.localScale.x * (fsmData.forwardDirection / currentLossyScaleX), 1, 1);
            fsmData.attackData = attackData;            
            fsmData.hitAttribute = attacker.FSMBrain.GetAIData<UnitFSMData>().attackAttribute;
        }
    }
}