using H00N.AI.FSM;
using ShibaInspector.Collections;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class UnitAttackEventListener : MonoBehaviour
    {
        [SerializeField] FSMBrain fsmBrain = null;
        [SerializeField] SerializableDictionary<EAttackFeedback, FSMState> attackFeedbackTable = null;

        private UnitAttackFeedbackFSMData fsmData = new UnitAttackFeedbackFSMData();

        public void Initialize()
        {
            fsmData = fsmBrain.GetAIData<UnitAttackFeedbackFSMData>();
        }

        public void OnAttack(EAttackFeedback feedback, float feedbackValue)
        {
            if (attackFeedbackTable.TryGetValue(feedback, out FSMState state) == false)
                return;

            fsmData.feedbackValue = feedbackValue;
            fsmBrain.ChangeState(state);
        }
    }
}