using H00N.AI.FSM;
using ShibaInspector.Collections;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class UnitAttackEventListener : MonoBehaviour
    {
        [SerializeField] FSMBrain fsmBrain = null;
        [SerializeField] SerializableDictionary<EAttackFeedback, FSMState> attackFeedbackTable = null;

        private UnitFSMData fsmData = new UnitFSMData();

        public void Initialize()
        {
            fsmData = fsmBrain.GetAIData<UnitFSMData>();
        }

        public void OnAttack(Unit attacker, EAttackFeedback feedback, float feedbackValue)
        {
            if (attackFeedbackTable.TryGetValue(feedback, out FSMState state) == false)
                return;

            int forwardDirection = attacker.transform.position.x > transform.position.x ? 1 : -1;
            fsmData.forwardDirection = forwardDirection;
            fsmData.unit.transform.localScale = new Vector3(fsmData.forwardDirection, 1, 1);

            fsmData.feedbackValue = feedbackValue;
            fsmBrain.ChangeState(state);
        }
    }
}