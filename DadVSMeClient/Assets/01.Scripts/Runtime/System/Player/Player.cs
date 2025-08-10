using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe.Players
{
    public class Player : Unit
    {
        [Header("Player")]
        [SerializeField] EnemyDetector enemyDetector = null;

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

        #if UNITY_EDITOR
        [ContextMenu("Set FSM State as Default State")]
        private void SetFSMStateAsDefaultState()
        {
            fsmBrain.SetAsDefaultState();
        }
        #endif
    }
}
