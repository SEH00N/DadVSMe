using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class PlayerInnerDistanceDecision : FSMDecision
    {
        [SerializeField] float distance = 0f;

        private EnemyFSMData enemyFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            enemyFSMData = brain.GetAIData<EnemyFSMData>();
        }

        public override bool MakeDecision()
        {
            float sqrDistance = ((Vector2)brain.transform.position - (Vector2)enemyFSMData.player.transform.position).sqrMagnitude;
            return sqrDistance < distance * distance;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(UnityEditor.Selection.activeObject != gameObject)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distance);
        }
        #endif
    }
}
