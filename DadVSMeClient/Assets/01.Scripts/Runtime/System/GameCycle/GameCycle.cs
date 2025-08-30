using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Background;
using DadVSMe.Players;
using DadVSMe.UI.HUD;
using NavMeshPlus.Components;
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

        [SerializeField] BackgroundTrailer backgroundTrailer = null;
        [SerializeField] NavMeshSurface navMeshSurface = null;

        [Space(10f)]
        [SerializeField] Transform startLine = null;
        public Transform StartLine => startLine;

        [SerializeField] Transform endLine = null;
        public Transform EndLine => endLine;

        [Space(10f)]
        [SerializeField] HUDUI hudUI = null;
        public HUDUI HUDUI => hudUI;

        public bool IsPaused { get; private set; } = false;
        public bool IsBossClearDirecting { get; private set; } = false;

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

            _ = new ChangeCinemachineCamera(mainCinemachineCamera);

            backgroundTrailer.onSpawnedBackground += HandleSpawnedBackground;
            
            return UniTask.CompletedTask;
        }

        private void HandleSpawnedBackground(int currentThemeIndex)
        {
            navMeshSurface.BuildNavMeshAsync();
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void SetBossClearDirecting(bool isBossClearDirecting)
        {
            IsBossClearDirecting = isBossClearDirecting;
        }
    }
}