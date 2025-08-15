using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class OverrideAttackDataAction : FSMAction
    {
        [SerializeField] ScriptableObject attackData = null;

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            unitFSMData.attackData = attackData as IAttackData;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if(attackData == null)
                return;

            if(attackData is IAttackData)
                return;

            Debug.LogError($"Attack data should be derived from IAttackData: {attackData.name}");
            attackData = null;
        }
        #endif
    }
}
