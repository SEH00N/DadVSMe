using Cysharp.Threading.Tasks;
using DadVSMe.Enemies;
using DadVSMe.Entities;
using DadVSMe.Inputs;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using Unity.Cinemachine;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class BossWaveBehaviour : WaveBehaviour
    {
        private static readonly Vector2 BOSS_DEAD_CAMERA_OFFSET = new Vector2(0f, 1f);

        /*private const*/ [SerializeField] float BOSS_SPAWN_ZOOM_TIME = 3f;
        /*private const*/ [SerializeField] float BOSS_SPAWN_ZOOM_TIME_SCALE = 0.25f;
        /*private const*/ [SerializeField] float BOSS_SPAWN_BLEND_TIME = 1.2f;

        /*private const*/ [SerializeField] float BOSS_DEAD_ZOOM_TIME = 3f;
        /*private const*/ [SerializeField] float BOSS_DEAD_ZOOM_TIME_SCALE = 0.25f;
        /*private const*/ [SerializeField] float BOSS_DEAD_BLEND_TIME = 2.2f;

        /*private const*/ [SerializeField] float BOSS_WAVE_BLEND_TIME = 2f;

        [Header("Options")]
        [SerializeField] GameObject bossWaveBlockObject = null;
        [SerializeField] Transform bossSpawnPoint = null;
        [SerializeField] float conditionDistance = 10f;

        [Header("Spawn Info")]
        [SerializeField] AddressableAsset<Unit> bossPrefab = null;
        [SerializeField] AddressableAsset<EnemyDataBase> enemyData = null;
        [SerializeField] AddressableAsset<UnitStatData> statData = null;

        [Header("Directing")]
        [SerializeField] AddressableAsset<BGMAudioLibrary> bgmLibrary = null;
        [SerializeField] CinemachineCamera bossSpawnCinemachineCamera = null;
        [SerializeField] CinemachineCamera bossDeadCinemachineCamera = null;
        [SerializeField] CinemachineCamera bossWaveCinemachineCamera = null;

        private Unit bossUnit = null;

        private void Awake()
        {
            bossPrefab.InitializeAsync().Forget();
            enemyData.InitializeAsync().Forget();
            statData.InitializeAsync().Forget();
            bgmLibrary.InitializeAsync().Forget();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

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

            // Play Boss Profile UI Directing
            // should be awaiting

            // Release Camera
            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true, 0.5f);
            _ = new ChangeCinemachineCamera(bossWaveCinemachineCamera, BOSS_WAVE_BLEND_TIME);
            bossWaveCinemachineCamera.Follow = GameInstance.CameraLookTransform;

            // Release Player
            GameInstance.GameCycle.MainPlayer.SetHold(false);

            // Release Boss
            bossUnit.SetHold(false);
        }

        private void HandleBossHPChanged()
        {
            if(bossUnit.GetComponent<UnitHealth>().CurrentHP > 0)
                return;

            bossUnit.UnitHealth.OnHPChangedEvent -= HandleBossHPChanged;
            HandleBossDead();
        }

        private async void HandleBossDead()
        {
            GameInstance.GameCycle.Pause();

            bossWaveBlockObject.SetActive(false);

            // Set Player Hold
            InputManager.DisableInput();

            // Play Camera Trnasitioning
            TimeManager.SetTimeScale(BOSS_DEAD_ZOOM_TIME_SCALE, true, 0.5f);
            bossDeadCinemachineCamera.Follow = bossUnit.transform;
            _ = new ChangeCinemachineCamera(bossDeadCinemachineCamera, BOSS_DEAD_BLEND_TIME);

            bossUnit = null;

            await UniTask.WaitForSeconds(BOSS_DEAD_ZOOM_TIME, ignoreTimeScale: true);

            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true, 0.5f);
            await GameInstance.GameCycle.Deadline.PlayBossClearDirecting();

            InputManager.EnableInput<PlayerInputReader>();

            GameInstance.GameCycle.Resume();
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
