using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class EnemyStateDecision : FSMDecision
    {
        [SerializeField] Transform pivot = null;
        [SerializeField] float distance = 3f;
        [SerializeField] bool includeStaticEnemy = true;

        [Space(10f)]
        [SerializeField] bool checkFloat = false;
        [SerializeField] bool isFloat = false;

        [Space(10f)]
        [SerializeField] bool checkLie = false;
        [SerializeField] bool isLie = false;

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

                bool result = true;
                float sqrDistance = (pivot.position - enemy.transform.position).sqrMagnitude;
                result &= sqrDistance < distance * distance;

                if(checkFloat)
                    result &= enemy.FSMBrain.GetAIData<UnitFSMData>().isFloat == isFloat;

                if(checkLie)
                    result &= enemy.FSMBrain.GetAIData<UnitFSMData>().isLie == isLie;

                if(result)
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
            Gizmos.DrawWireSphere(pivot.position, distance);
        }
        #endif
    }
}
