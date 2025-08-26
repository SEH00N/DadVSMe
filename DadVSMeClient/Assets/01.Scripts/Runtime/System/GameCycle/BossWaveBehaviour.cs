using Cysharp.Threading.Tasks;
using DadVSMe.Enemies;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using Unity.Cinemachine;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class BossWaveBehaviour : WaveBehaviour
    {
        private const float BOSS_ZOOM_TIME = 3f;
        private const float BOSS_ZOOM_TIME_SCALE = 0.5f;

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
        [SerializeField] CinemachineCamera bossZoomCinemachineCamera = null;
        [SerializeField] CinemachineCamera bossWaveCinemachineCamera = null;

        private Unit bossUnit = null;

        private void Awake()
        {
            bossPrefab.InitializeAsync().Forget();
            enemyData.InitializeAsync().Forget();
            statData.InitializeAsync().Forget();
            bgmLibrary.InitializeAsync().Forget();
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
            Time.timeScale = BOSS_ZOOM_TIME_SCALE;
            _ = new ChangeCinemachineCamera(bossZoomCinemachineCamera);

            await UniTask.WaitForSeconds(BOSS_ZOOM_TIME, ignoreTimeScale: true);

            // Play Boss Profile UI Directing
            // should be awaiting

            // Release Camera
            Time.timeScale = GameDefine.DEFAULT_TIME_SCALE;
            _ = new ChangeCinemachineCamera(bossWaveCinemachineCamera);
            bossWaveCinemachineCamera.Follow = GameInstance.CameraLookTransform;

            // Release Player
            GameInstance.GameCycle.MainPlayer.SetHold(false);

            // Release Boss
            bossUnit.SetHold(false);
        }

        private void HandleBossDead()
        {
            bossWaveBlockObject.SetActive(false);

            // Play Dad Springing Up Animation

            // Player Juggling

            // Go! Text

            _ = new ChangeCinemachineCamera(GameInstance.GameCycle.MainCinemachineCamera);
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
