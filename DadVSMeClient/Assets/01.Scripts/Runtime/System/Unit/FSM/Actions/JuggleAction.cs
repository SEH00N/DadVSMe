using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class JuggleAction : HitAction
    {
        [SerializeField] FSMState bounceState = null;
        private Rigidbody2D unitRigidbody = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitRigidbody = brain.GetComponent<Rigidbody2D>();
        }

        public override void EnterState()
        {
            base.EnterState();

            JuggleAttackData juggleAttackData = attackData as JuggleAttackData;
            if(juggleAttackData == null)
                return;

            unitFSMData.unit.SetFloat(true);

            Vector2 force = juggleAttackData.JuggleDirection.normalized * juggleAttackData.JuggleForce;
            force.x *= -Mathf.Sign(unitFSMData.forwardDirection);
            unitRigidbody.linearVelocity = force;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            // if the bounce is not due to a collision, the bounce is checked based on the last ground position.
            if(brain.transform.position.y >= unitFSMData.groundPositionY || unitRigidbody.linearVelocity.y > 0)
                return;

            unitFSMData.collisionData = new UnitCollisionData(unitRigidbody.linearVelocity, Vector2.up, new Vector2(brain.transform.position.x, unitFSMData.groundPositionY));
            brain.ChangeState(bounceState);
        }
    }
}