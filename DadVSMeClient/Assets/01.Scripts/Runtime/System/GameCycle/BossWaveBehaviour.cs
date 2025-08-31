using Cysharp.Threading.Tasks;
using DadVSMe.Enemies;
using DadVSMe.Entities;
using DadVSMe.Inputs;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DadVSMe.GameCycles
{
    public class BossWaveBehaviour : WaveBehaviour
    {
        private const float BOSS_SPAWN_ZOOM_TIME = 1.25f;
        private const float BOSS_SPAWN_ZOOM_TIME_SCALE = 0.2f;
        private const float BOSS_SPAWN_BLEND_TIME = 1.3f;

        private const float BOSS_DEAD_ZOOM_TIME = 2.5f;
        private const float BOSS_DEAD_ZOOM_TIME_SCALE = 0.25f;
        private const float BOSS_DEAD_BLEND_TIME = 2.2f;

        private const float BOSS_WAVE_BLEND_TIME = 2f;
        private const float BOSS_WAVE_MAIN_CAMERA_BLEND_TIME = 0.5f;
        private const float BOSS_WAVE_MAIN_CAMERA_RELEASE_DURATION = 0.5f;

        [Header("Options")]
        [SerializeField] GameObject bossWaveBlockObject = null;
        [SerializeField] Transform bossSpawnPoint = null;
        [SerializeField] float conditionDistance = 10f;
        [SerializeField] bool isFinalBoss = false;

        [Header("Spawn Info")]
        [SerializeField] AddressableAsset<Unit> bossPrefab = null;
        [SerializeField] AddressableAsset<EnemyDataBase> enemyData = null;
        [SerializeField] AddressableAsset<UnitStatData> statData = null;

        [Header("Directing")]
        [SerializeField] AddressableAsset<BGMAudioLibrary> bgmLibrary = null;
        [SerializeField] CinemachineCamera bossSpawnCinemachineCamera = null;
        [SerializeField] CinemachineCamera bossDeadCinemachineCamera = null;
        [SerializeField] CinemachineCamera bossWaveCinemachineCamera = null;

        [SerializeField] AddressableAsset<BossProductionUI> productionPrefab = null;
        [SerializeField] Sprite bossProfileVisual = null;

        private Unit bossUnit = null;

        private void Awake()
        {
            bossPrefab.InitializeAsync().Forget();
            enemyData.InitializeAsync().Forget();
            statData.InitializeAsync().Forget();
            bgmLibrary.InitializeAsync().Forget();
            productionPrefab.InitializeAsync().Forget();
        }

        protected override void PostOnEnable()
        {
            base.PostOnEnable();
            bossWaveBlockObject.SetActive(false);
        }

        protected override void PostOnDisable()
        {
            base.PostOnDisable();

            if(bossUnit != null && bossUnit.UnitHealth != null)
            {
                bossUnit.UnitHealth.OnHPChangedEvent -= HandleBossHPChanged;
                bossUnit = null;
            }
        }

        protected override bool OnUpdate()
        {
            if(GameInstance.GameCycle == null || GameInstance.GameCycle.MainPlayer == null)
                return false;

            Vector2 direction = GameInstance.GameCycle.MainPlayer.transform.position - bossSpawnPoint.position;
            if(direction.sqrMagnitude >= conditionDistance * conditionDistance)
                return false;

            SpawnBoss();
            return true;
        }

        private void SpawnBoss()
        {
            bossUnit = PoolManager.Spawn<Unit>(bossPrefab.Key, GameInstance.GameCycle.transform);
            bossUnit.transform.position = bossSpawnPoint.position;
            bossUnit.FSMBrain.SetAIData(statData.Asset);
            bossUnit.Initialize(enemyData.Asset);
            bossUnit.UnitHealth.OnHPChangedEvent += HandleBossHPChanged;

            PlayBossDirecting();
        }
    
        private async void PlayBossDirecting()
        {
            // Play Audio
            AudioManager.Instance.PlayBGM(bgmLibrary, loadCache: false);

            // Set Player Hold
            GameInstance.GameCycle.MainPlayer.SetHold(true);

            // Set Boss Hold
            bossUnit.SetHold(true);

            // Play Camera Trnasitioning
            TimeManager.SetTimeScale(BOSS_SPAWN_ZOOM_TIME_SCALE, true, 0.5f);
            _ = new ChangeCinemachineCamera(bossSpawnCinemachineCamera, BOSS_SPAWN_BLEND_TIME);

            await UniTask.WaitForSeconds(BOSS_SPAWN_ZOOM_TIME, ignoreTimeScale: true);

            // Set Deadline Inactive
            GameInstance.GameCycle.Deadline.SetActive(false);

            // Play Boss Profile UI Directing
            // should be awaiting
            var production = PoolManager.Spawn<BossProductionUI>(productionPrefab, GameInstance.MainPopupFrame);
            production.StretchRect();
            await production.Initialize(bossProfileVisual);
            PoolManager.Despawn(production);

            // Release Camera
            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true, 0.5f);
            _ = new ChangeCinemachineCamera(bossWaveCinemachineCamera, BOSS_WAVE_BLEND_TIME);
            bossWaveCinemachineCamera.Follow = GameInstance.CameraLookTransform;

            // Release Player
            GameInstance.GameCycle.MainPlayer.SetHold(false);

            // Release Boss
            bossUnit.SetHold(false);

            // Set Boss Wave Block Object Active
            bossWaveBlockObject.SetActive(true);
        }

        private void HandleBossHPChanged()
        {
            if(bossUnit.GetComponent<UnitHealth>().CurrentHP > 0)
                return;

            bossUnit.UnitHealth.OnHPChangedEvent -= HandleBossHPChanged;
            HandleBossDead();
        }

        private void HandleBossDead()
        {
            if(isFinalBoss)
                EndingDirecting();
            else
                PlayBossClearDirecting();
        }

        private async void PlayBossClearDirecting()
        {
            GameInstance.GameCycle.Pause();
            InputManager.DisableInput();

            GameInstance.GameCycle.SetBossClearDirecting(true);
            AudioManager.Instance.PlayBGM(GameInstance.GameCycle.MainBGMLibrary, loadCache: true);

            bossWaveBlockObject.SetActive(false);

            // Play Camera Trnasitioning
            TimeManager.SetTimeScale(BOSS_DEAD_ZOOM_TIME_SCALE, true, 0.5f);
            bossDeadCinemachineCamera.Follow = bossUnit.transform;
            _ = new ChangeCinemachineCamera(bossDeadCinemachineCamera, BOSS_DEAD_BLEND_TIME);

            bossUnit = null;

            await UniTask.WaitForSeconds(BOSS_DEAD_ZOOM_TIME, ignoreTimeScale: true);

            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true, 0.5f);
            await GameInstance.GameCycle.Deadline.PlayBumpPlayerDirecting(BOSS_WAVE_MAIN_CAMERA_BLEND_TIME, BOSS_WAVE_MAIN_CAMERA_RELEASE_DURATION);

            GameInstance.GameCycle.SetBossClearDirecting(false);

            InputManager.EnableInput<PlayerInputReader>();
            GameInstance.GameCycle.Resume();
        }

        private async void EndingDirecting()
        {
            GameInstance.GameCycle.Pause();
            InputManager.DisableInput();

            TimeManager.SetTimeScale(BOSS_DEAD_ZOOM_TIME_SCALE, true, 0.5f);
            bossDeadCinemachineCamera.Follow = bossUnit.transform;
            _ = new ChangeCinemachineCamera(bossDeadCinemachineCamera, BOSS_DEAD_BLEND_TIME);

            bossUnit = null;

            await UniTask.WaitForSeconds(BOSS_DEAD_ZOOM_TIME, ignoreTimeScale: true);

            await DOFade.FadeInAsync(2f);
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_CLEAR_SCENE_NAME, LoadSceneMode.Single);
            DOFade.FadeOutAsync(1f).Forget();

            TimeManager.SetTimeScale(2f, true);

            InputManager.EnableInput<PlayerInputReader>();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(bossSpawnPoint == null)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(bossSpawnPoint.position, conditionDistance);
        }
        #endif
    }
}
