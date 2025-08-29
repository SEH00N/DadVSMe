using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class EntityBoundary : MonoBehaviour, IAttacker
    {
        [SerializeField] JuggleAttackData collisionAttackData = null;

        public Transform AttackerTransform => transform;
        public EAttackAttribute AttackAttribute => EAttackAttribute.Normal;
        public float AttackPower => 1f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<Unit>(out var unit) == false)
                return;

            FSMBrain unitFSMBrain = unit.FSMBrain;
            if(unitFSMBrain.GetAIData<UnitFSMData>().isFloat == false)
                return;

            unit.UnitHealth.Attack(this, collisionAttackData);
        }
    }
}
