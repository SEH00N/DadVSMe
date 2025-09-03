using System.Collections.Generic;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class JuggleAction : HitAction
    {
        private const float JUGGLE_ANGULAR_VELOCITY = 180f;

        [SerializeField] FSMState bounceState = null;
        private Rigidbody2D unitRigidbody = null;

        private HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitRigidbody = brain.GetComponent<Rigidbody2D>();
        }

        public override void EnterState()
        {
            base.EnterState();

            hitColliders.Clear();

            if (attackData is IJuggleAttackData juggleAttackData == false)
                return;

            unitFSMData.unit.SetFloat(true);

            Vector2 force = juggleAttackData.JuggleDirection.normalized * juggleAttackData.JuggleForce;
            force.x *= -Mathf.Sign(unitFSMData.forwardDirection);
            unitRigidbody.linearVelocity = force;
            unitRigidbody.angularVelocity = JUGGLE_ANGULAR_VELOCITY;
            unitRigidbody.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        }

        public override void ExitState()
        {
            base.ExitState();
            
            brain.transform.rotation = Quaternion.identity;
            unitRigidbody.angularVelocity = 0f;
            unitRigidbody.constraints |= RigidbodyConstraints2D.FreezeRotation;

            unitFSMData.OnBowlingEvent = null;
            hitColliders.Clear();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Collider2D unitCollider = unitFSMData.unit.UnitCollider;
            Collider2D[] cols = Physics2D.OverlapBoxAll(unitCollider.bounds.center, unitCollider.bounds.size, unitFSMData.unit.transform.eulerAngles.z, GameDefine.ENEMY_LAYER_MASK | GameDefine.PLAYER_LAYER_MASK);
            foreach (var col in cols)
            {
                if(hitColliders.Contains(col))
                    continue;

                hitColliders.Add(col);

                if(col.TryGetComponent<Unit>(out Unit unit))
                    unitFSMData.OnBowlingEvent?.Invoke(unit);
            }
        }
    }
}