using Cysharp.Threading.Tasks;
using DadVSMe.Inputs;
using DadVSMe.UI;
using DadVSMe.UI.Setting;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class PauseEventListner : MonoBehaviour, PausePopupUI.ICallback
    {
        private const string BLOCK_KEY = "PauseEventListner";

        [SerializeField] AddressableAsset<PausePopupUI> _pausePopupPrefab = null;
        [SerializeField] AddressableAsset<SettingPopupUI> _settingPopupPrefab = null;
        // private PlayerInputReader _playerInputReader = null;
        private UnitSkillComponent _unitSkillComponent = null;

        private PausePopupUI _pausePopupUI = null;
        private SettingPopupUI _settingPopupUI = null;

        private async void Start()
        {
            await _pausePopupPrefab.InitializeAsync();

            // _playerInputReader = InputManager.GetInput<PlayerInputReader>();
            // HandleSetupSpawnPausePopupUI();

            PlayerInputReader playerInputReader = InputManager.GetInput<PlayerInputReader>();
            if(playerInputReader == null)
                return;

            playerInputReader.OnPressPauseEvent -= HandlePauseEvent;
            playerInputReader.OnPressPauseEvent += HandlePauseEvent;
        }

        private void OnDestroy()
        {
            PlayerInputReader playerInputReader = InputManager.GetInput<PlayerInputReader>();
            if(playerInputReader == null)
                return;

            playerInputReader.OnPressPauseEvent -= HandlePauseEvent;
        }

        private async void HandlePauseEvent()
        {
            // Handle Unity Null Expression
            if(this == null)
                return;

            // _playerInputReader.OnPressPauseEvent -= HandleSpawnPausePopupUI;

            if(_settingPopupUI != null)
            {
                await _settingPopupUI.OnTouchConfirmButtonAsync();
                _settingPopupUI = null;
            }
            else if(_pausePopupUI != null)
            {
                await _pausePopupUI.ResumeInternalAsync();
                _pausePopupUI = null;
            }
            else
            {
                SpawnPausePopupUI();
            }
        }

        private void SpawnPausePopupUI()
        {
            if(Time.timeScale != GameDefine.DEFAULT_TIME_SCALE)
                return;

            if(_unitSkillComponent == null)
                _unitSkillComponent = GameInstance.GameCycle.MainPlayer.GetComponent<UnitSkillComponent>();

            _pausePopupUI = PoolManager.Spawn<PausePopupUI>(_pausePopupPrefab, GameInstance.MainPopupFrame);
            _pausePopupUI.StretchRect();
            _pausePopupUI.Initialize(_unitSkillComponent, this);            
        }

        // private void HandleSetupSpawnPausePopupUI()
        // {
        //     _playerInputReader.OnPressPauseEvent -= HandleSpawnPausePopupUI;
        //     _playerInputReader.OnPressPauseEvent += HandleSpawnPausePopupUI;
        // }

        public void OnRelease()
        {
            _pausePopupUI = null;
            _settingPopupUI = null;
        }

        public async void OnTouchSettingButton()
        {
            InputBlock.Block(BLOCK_KEY);
            await _settingPopupPrefab.InitializeAsync();
            InputBlock.Release(BLOCK_KEY);

            _settingPopupUI = PoolManager.Spawn<SettingPopupUI>(_settingPopupPrefab, GameInstance.MainPopupFrame);
            _settingPopupUI.StretchRect();
            _settingPopupUI.Initialize().Forget();
        }
    }
}
