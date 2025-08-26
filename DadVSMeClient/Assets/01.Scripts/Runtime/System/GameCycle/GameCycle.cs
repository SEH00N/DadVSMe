using Cysharp.Threading.Tasks;
using DadVSMe.Players;
using DadVSMe.UI.HUD;
using Unity.Cinemachine;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class GameCycle : MonoBehaviour
    {
        [SerializeField] CinemachineCamera mainCinemachineCamera = null;
        public CinemachineCamera MainCinemachineCamera => mainCinemachineCamera;

        [Space(10f)]
        [SerializeField] Deadline deadline = null;
        public Deadline Deadline => deadline;
        
        [SerializeField] Player mainPlayer = null;
        public Player MainPlayer => mainPlayer;

        [Space(10f)]
        [SerializeField] Transform startLine = null;
        public Transform StartLine => startLine;

        [SerializeField] Transform endLine = null;
        public Transform EndLine => endLine;

        [Space(10f)]
        [SerializeField] HUDUI hudUI = null;

        public bool IsPaused { get; private set; } = false;

        // Debug
        private void Start()
        {
            InitializeAsync().Forget();
        }

        public UniTask InitializeAsync()
        {
            deadline.Initialize();
            MainPlayer.Initialize(new PlayerEntityData());
            hudUI.Initialize();
            return UniTask.CompletedTask;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }
    }
}