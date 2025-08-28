using DadVSMe.Inputs;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using System;
using UnityEngine;

namespace DadVSMe
{
    public class PauseListner : MonoBehaviour
    {
        [SerializeField] AddressableAsset<PausePopupUI> _pausePopupPrefab = null;
        private PlayerInputReader _playerInputReader = null;
        private UnitSkillComponent _unitSkillComponent = null;

        private PausePopupUI _pausePopupUI = null;

        private async void Start()
        {
            await _pausePopupPrefab.InitializeAsync();

            _playerInputReader = InputManager.GetInput<PlayerInputReader>();
            HandleSetupSpawnPausePopupUI();
        }

        private void HandleSpawnPausePopupUI()
        {
            _playerInputReader.onPressPause -= HandleSpawnPausePopupUI;

            _unitSkillComponent ??= GameInstance.GameCycle.MainPlayer.GetComponent<UnitSkillComponent>();

            _pausePopupUI = PoolManager.Spawn<PausePopupUI>(_pausePopupPrefab, GameInstance.MainPopupFrame);
            _pausePopupUI.StretchRect();
            _pausePopupUI.Initialize(_unitSkillComponent, HandleSetupSpawnPausePopupUI);
        }

        private void HandleSetupSpawnPausePopupUI()
        {
            _playerInputReader.onPressPause -= HandleSpawnPausePopupUI;
            _playerInputReader.onPressPause += HandleSpawnPausePopupUI;
        }
    }
}
