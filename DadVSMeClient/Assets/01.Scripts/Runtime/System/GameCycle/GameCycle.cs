using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DadVSMe.Players;
using DadVSMe.UI.HUD;
using H00N.Resources.Addressables;
using Unity.Cinemachine;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class GameCycle : MonoBehaviour
    {
        [SerializeField] CinemachineCamera mainCinemachineCamera = null;
        public CinemachineCamera MainCinemachineCamera => mainCinemachineCamera;
        
        [SerializeField] HUDUI hudUI = null;
        public HUDUI HUDUI => hudUI;

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
        [SerializeField] AddressableAsset<BGMAudioLibrary> mainBGMLibrary = null;
        public AddressableAsset<BGMAudioLibrary> MainBGMLibrary => mainBGMLibrary;

        public bool IsPaused { get; private set; } = false;
        public bool IsBossClearDirecting { get; private set; } = false;

        // Debug
        private void Start()
        {
            InitializeAsync().Forget();
        }

        public async UniTask InitializeAsync()
        {
            deadline.Initialize();
            MainPlayer.Initialize(new PlayerEntityData());
            hudUI.Initialize();

            _ = new ChangeCinemachineCamera(mainCinemachineCamera);

            await mainBGMLibrary.InitializeAsync();
            AudioManager.Instance.PlayBGM(mainBGMLibrary, loadCache: false);
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