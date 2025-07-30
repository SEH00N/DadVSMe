using System.Linq;
using DadVSMe.Enemies;
using H00N.AI.FSM;
using UnityEditor;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class EnemyInnerDistanceDecision : FSMDecision
    {
        [SerializeField] Transform pivot = null;
        [SerializeField] float distance = 3f;

        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override bool MakeDecision()
        {
            if(fsmData.enemies.Count == 0)
                return false;

            Enemy enemy = fsmData.enemies[0];
            if(enemy == null)
                return false;

            float sqrDistance = (pivot.position - enemy.transform.position).sqrMagnitude;
            return sqrDistance < distance * distance;
        }

        [Header("Debug")]
        [SerializeField] bool drawGizmos = false;
        [SerializeField] bool drawSelected = false;
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(drawGizmos == false)
                return;

            if(drawSelected && Selection.objects.Contains(gameObject) == false)
                return;

            if(pivot == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pivot.position, distance);
        }
        #endif
    }
}
