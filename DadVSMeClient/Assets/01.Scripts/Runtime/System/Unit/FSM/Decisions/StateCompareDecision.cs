using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class StateCompareDecision : FSMDecision
    {
        [SerializeField] FSMState compareState = null;

        public override bool MakeDecision()
        {
            return brain.CurrentState == compareState;
        }
    }
}
