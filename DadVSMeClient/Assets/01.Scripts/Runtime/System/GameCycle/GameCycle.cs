using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DadVSMe.Inputs;
using DadVSMe.Players;
using DadVSMe.UI.HUD;
using DG.Tweening;
using H00N.Resources.Addressables;
using Unity.Cinemachine;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class GameCycle : MonoBehaviour
    {
        private const float GAME_START_TIME_SCALE = 0.3f;
        private const float GAME_START_TIME_SCALE_BLEND_DURATION = 10f;
        private const float GAME_START_DEADLINE_MOVE_DISTANCE = 50f;
        private const float GAME_START_DEADLINE_MOVE_GAP = 7.5f;
        private const float GAME_START_DEADLINE_MOVE_DURATION = 1.225f;
        private const float GAME_START_DEADLINE_MOVE_WAIT_DURATION = 0.85f;
        private const float GAME_START_MAIN_CAMERA_BLEND_DURATION = 0.45f;
        private const float GAME_START_MAIN_CAMERA_RELEASE_DURATION = -0.1f;
        private const float DEADLINE_SPEED = 2f;

        [SerializeField] CinemachineCamera mainCinemachineCamera = null;
        public CinemachineCamera MainCinemachineCamera => mainCinemachineCamera;

        [SerializeField] CinemachineCamera characterZoomCinemachineCamera = null;
        public CinemachineCamera CharacterZoomCinemachineCamera => characterZoomCinemachineCamera;

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

        #if UNITY_EDITOR
        // Debug
        private void Start()
        {
            InitializeAsync().Forget();
        }
        #endif

        public async UniTask InitializeAsync()
        {
            deadline.Initialize();
            MainPlayer.Initialize(new PlayerEntityData());
            hudUI.Initialize();

            await mainBGMLibrary.InitializeAsync();
            AudioManager.Instance.PlayBGM(mainBGMLibrary, loadCache: false);

            await PlayGameStartDirecting();
        }

        private async UniTask PlayGameStartDirecting()
        {
            deadline.SetSpeed(0f);
            InputManager.DisableInput();

            TimeManager.SetTimeScale(GAME_START_TIME_SCALE, true);
            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true, GAME_START_TIME_SCALE_BLEND_DURATION);

            _ = new ChangeCinemachineCamera(characterZoomCinemachineCamera, 0f);

            Vector3 targetPosition = new Vector3(startLine.position.x - GAME_START_DEADLINE_MOVE_GAP, deadline.transform.position.y, deadline.transform.position.z);
            Vector3 startPosition = targetPosition + Vector3.left * (GAME_START_DEADLINE_MOVE_DISTANCE - GAME_START_DEADLINE_MOVE_GAP);
            deadline.transform.position = startPosition;

            _ = deadline.transform.DOMove(targetPosition, GAME_START_DEADLINE_MOVE_DURATION).SetEase(Ease.InQuint);
            await UniTask.WaitForSeconds(GAME_START_DEADLINE_MOVE_WAIT_DURATION);
            // _ = new ChangeCinemachineCamera(mainCinemachineCamera, 1f);
            // await UniTask.WaitForSeconds(1f);

            await deadline.PlayBumpPlayerDirecting(GAME_START_MAIN_CAMERA_BLEND_DURATION, GAME_START_MAIN_CAMERA_RELEASE_DURATION);
            deadline.SetSpeed(DEADLINE_SPEED);
            InputManager.EnableInput<PlayerInputReader>();
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