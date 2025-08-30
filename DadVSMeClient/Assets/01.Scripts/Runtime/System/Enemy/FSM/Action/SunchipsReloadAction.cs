using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsReloadAction : FSMAction
    {
        [SerializeField] private float cooltime;

        private SunchipsEnemyFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<SunchipsEnemyFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            fsmData.shootCount = 0;
            fsmData.shootCooltime = cooltime;
        }
    }
}
