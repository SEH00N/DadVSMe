using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe.Players
{
    public class Player : Unit
    {
        [Header("Player")]
        [SerializeField] EnemyDetector enemyDetector = null;

        protected override RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Dynamic;

        // Debug
        private void Start()
        {
            Initialize(null);
        }

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            enemyDetector.Initialize();
        }

        #if UNITY_EDITOR
        [ContextMenu("Set FSM State as Default State")]
        private void SetFSMStateAsDefaultState()
        {
            fsmBrain.SetAsDefaultState();
        }
        #endif
    }
}
