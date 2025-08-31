using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        [SerializeField] CinemachineCamera mainCinemachineCamera = null;
        public CinemachineCamera MainCinemachineCamera => mainCinemachineCamera;

        [SerializeField] CinemachineCamera characterZoomCinemachineCamera = null;
        
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

            await mainBGMLibrary.InitializeAsync();
            AudioManager.Instance.PlayBGM(mainBGMLibrary, loadCache: false);

            await PlayGameStartDirecting();
        }

        public float test = 2f;
        public float test1 = 1f;
        public float test2 = 0.75f;
        public float test3 = 0.2f;

        private async UniTask PlayGameStartDirecting()
        {
            deadline.SetSpeed(0f);

            TimeManager.SetTimeScale(0.3f, true);
            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true, 2f);

            _ = new ChangeCinemachineCamera(characterZoomCinemachineCamera, 0f);

            Vector3 targetPosition = new Vector3(startLine.position.x - 7.5f, deadline.transform.position.y, deadline.transform.position.z);
            Vector3 startPosition = targetPosition + Vector3.left * 42.5f;
            deadline.transform.position = startPosition;

            _ = deadline.transform.DOMove(targetPosition, test).SetEase(Ease.InQuint);
            await UniTask.WaitForSeconds(test1);
            // _ = new ChangeCinemachineCamera(mainCinemachineCamera, 1f);
            // await UniTask.WaitForSeconds(1f);

            await deadline.PlayBumpPlayerDirecting(test2, test3);
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