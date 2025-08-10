using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class EnemyInnerDistanceDecision : FSMDecision
    {
        [SerializeField] Transform pivot = null;
        [SerializeField] float distance = 3f;

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override bool MakeDecision()
        {
            if(unitFSMData.enemies.Count == 0)
                return false;

            Unit enemy = unitFSMData.enemies[0];
            if(enemy == null)
                return false;

            float sqrDistance = (pivot.position - enemy.transform.position).sqrMagnitude;
            return sqrDistance < distance * distance;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(UnityEditor.Selection.activeObject != gameObject)
                return;

            if(pivot == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pivot.position, distance);
        }
        #endif
    }
}
