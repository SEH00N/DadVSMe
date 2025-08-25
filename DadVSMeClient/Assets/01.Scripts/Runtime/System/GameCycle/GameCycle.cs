using Cysharp.Threading.Tasks;
using DadVSMe.Players;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class GameCycle : MonoBehaviour
    {
        [SerializeField] Deadline deadline = null;
        public Deadline Deadline => deadline;
        
        [SerializeField] Player mainPlayer = null;
        public Player MainPlayer => mainPlayer;

        [Space(10f)]
        [SerializeField] Transform startLine = null;
        public Transform StartLine => startLine;

        [SerializeField] Transform endLine = null;
        public Transform EndLine => endLine;

        // Debug
        private void Start()
        {
            InitializeAsync().Forget();
        }

        public UniTask InitializeAsync()
        {
            deadline.Initialize();
            MainPlayer.Initialize(new PlayerEntityData());
            return UniTask.CompletedTask;
        }
    }
}