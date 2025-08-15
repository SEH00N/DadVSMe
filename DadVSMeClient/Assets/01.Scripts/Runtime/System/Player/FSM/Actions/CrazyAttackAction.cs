using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class CrazyAttackAction : SimpleAttackAction
    {
        [Space(10f)]
        [SerializeField] Vector2 jumpForce = Vector2.zero;
        private Rigidbody2D unitRigidbody = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitRigidbody = brain.GetComponent<Rigidbody2D>();
        }

        public override void EnterState()
        {
            base.EnterState();
            
            if(unitFSMData.isFloat == false)
                unitFSMData.unit.SetFloat(true);

            unitRigidbody.linearVelocity = new Vector2(jumpForce.x * unitFSMData.forwardDirection, jumpForce.y);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(brain.transform.position.y >= unitFSMData.groundPositionY)
                return;

            brain.SetAsDefaultState();
        }

        public override void ExitState()
        {
            base.ExitState();

            if(unitFSMData.isFloat)
                unitFSMData.unit.SetFloat(false);
        }
    }
}
