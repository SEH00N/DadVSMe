using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class HPDecision : FSMDecision
    {
        [SerializeField] float hpThreshold = 0f;
        private UnitHealth unitHealth = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitHealth = brain.GetComponent<UnitHealth>();
        }

        public override bool MakeDecision()
        {
            return unitHealth.CurrentHP <= hpThreshold;
        }
    }
}
