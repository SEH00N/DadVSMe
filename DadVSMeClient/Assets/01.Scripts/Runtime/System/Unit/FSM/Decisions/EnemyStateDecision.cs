using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class EnemyStateDecision : FSMDecision
    {
        [SerializeField] Transform pivot = null;
        [SerializeField] UnitStateChecker unitStateChecker = null;

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

                if(unitStateChecker.Check(unitFSMData.unit, enemy, pivot) == false)
                    continue;

                return true;
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
            if(unitStateChecker.checkDirection)
            {
                if(unitStateChecker.directionMatch)
                {
                    Gizmos.DrawWireCube(pivot.position + Vector3.right * (Mathf.Sign(transform.localScale.x) * unitStateChecker.attackHorizontalDistance * 0.5f), new Vector3(unitStateChecker.attackHorizontalDistance, unitStateChecker.attackVerticalDistance * 2f, 0f));
                }
                else
                {
                    Gizmos.DrawWireCube(pivot.position - Vector3.right * (Mathf.Sign(transform.localScale.x) * unitStateChecker.attackHorizontalDistance * 0.5f), new Vector3(unitStateChecker.attackHorizontalDistance, unitStateChecker.attackVerticalDistance * 2f, 0f));
                }
            }
            else
            {
                Gizmos.DrawWireCube(pivot.position, new Vector3(unitStateChecker.attackHorizontalDistance * 2f, unitStateChecker.attackVerticalDistance * 2f, 0f));
            }
        }
        #endif
    }
}
