using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class Enemy : Unit, IGrabbable
    {
        [Header("Enemy")]
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] FSMState grabState = null;

        // Debug
        private void Start()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            enemyDetector.Initialize();
        }

        void IGrabbable.Grab(Entity performer)
        {
            performer.AddChildSortingOrderResolver(sortingOrderResolver);
            fsmBrain.ChangeState(grabState);
        }

        void IGrabbable.Release(Entity performer)
        {
            performer.RemoveChildSortingOrderResolver(sortingOrderResolver);
        }
    }
}
