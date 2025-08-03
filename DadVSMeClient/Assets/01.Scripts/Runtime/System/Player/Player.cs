using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] FSMBrain fsmBrain = null;
        [SerializeField] PlayerEnemyDetector enemyDetector = null;
        [SerializeField] PlayerAnimator playerAnimator = null;

        // Debug
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            playerAnimator.Initialize();
            enemyDetector.Initialize();

            fsmBrain.Initialize();
            fsmBrain.SetAsDefaultState();
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
