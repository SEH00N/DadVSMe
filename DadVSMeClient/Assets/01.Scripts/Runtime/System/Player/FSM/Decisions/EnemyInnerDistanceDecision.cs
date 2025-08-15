using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class EnemyInnerDistanceDecision : FSMDecision
    {
        [SerializeField] Transform pivot = null;
        [SerializeField] float distance = 3f;
        [SerializeField] bool includeStaticEnemy = true;

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

            foreach(Unit enemy in unitFSMData.enemies)
            {
                if(enemy == null)
                    continue;

                if(enemy.StaticEntity && includeStaticEnemy == false)
                    continue;

                float sqrDistance = (pivot.position - enemy.transform.position).sqrMagnitude;
                return sqrDistance < distance * distance;
            }

            return false;
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
