using UnityEngine;
using H00N.AI.FSM;

namespace DadVSMe.Entities.FSM
{
    public class UnitStateDecision : FSMDecision
    {
        [SerializeField] private bool checkFloat = false;
        [SerializeField] private bool isFloat = false;

        [Space(10f)]
        [SerializeField] private bool checkLie = false;
        [SerializeField] private bool isLie = false;

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override bool MakeDecision()
        {
            bool result = true;

            if(checkFloat)
                result &= unitFSMData.isFloat == isFloat;

            if(checkLie)
                result &= unitFSMData.isLie == isLie;

            return result;
        }
    }
}
