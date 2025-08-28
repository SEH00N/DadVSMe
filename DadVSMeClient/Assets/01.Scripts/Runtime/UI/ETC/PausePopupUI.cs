using Cysharp.Threading.Tasks;
using DadVSMe.UI;
using DadVSMe.UI.Setting;
using DadVSMe.UI.Skills;
using DG.Tweening;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DadVSMe
{
    public class PausePopupUI : PoolableBehaviourUI
    {
        [Header("Animation")]
        [SerializeField] Image _backgroundDimmed;
        [SerializeField] Transform _panelTransform;

        private const float APPEAR_TIME = 0.3f;
        private const float DISAPPEAR_TIME = 0.2f;
        private const float DIMMED_VALUE = 0.5f;

        [SerializeField] AddressableAsset<SkillInfoElementUI> _skillInfoElementPrefab = null;
        [SerializeField] Transform _contentTransform = null;
        [SerializeField] AddressableAsset<SettingPopupUI> _settingPopupPrefab = null;

        private const string BLOCK_KEY = "PausePopupUI";

        private Action _realseAction;

        public async void Initialize(UnitSkillComponent unitSkillComponent, Action releaseAction)
        {
            Color color = _backgroundDimmed.color;
            color.a = 0;
            _backgroundDimmed.color = color;

            _panelTransform.localScale = new Vector2(0, 1);

            await _skillInfoElementPrefab.InitializeAsync();

            Time.timeScale = 0f;

            foreach (SkillType skillType in unitSkillComponent)
            {
                SkillData skillData = unitSkillComponent.SkillDataContainer.GetSkillData(skillType);
                UnitSkill unitSkill = unitSkillComponent.GetSkill(skillType);

                if(unitSkill == null) continue;

                var element = PoolManager.Spawn<SkillInfoElementUI>(_skillInfoElementPrefab, _contentTransform);
                element.Initialize(skillData, unitSkill.Level);
            }

            _realseAction = null;
            _realseAction += releaseAction;
            _realseAction += HandleRelease;

            InputBlock.Block(BLOCK_KEY);
            _ = _backgroundDimmed.DOFade(DIMMED_VALUE, APPEAR_TIME).SetUpdate(true);
            _ = _panelTransform.DOScale(Vector2.one, APPEAR_TIME).SetEase(Ease.OutBack).SetUpdate(true);
            await UniTask.Delay(TimeSpan.FromSeconds(APPEAR_TIME), true);
            InputBlock.Release(BLOCK_KEY);
        }

        public async void OnTouchTitleButton()
        {
            await DOFade.FadeInAsync();

            await new ReleaseResourceByLabel().ReleaseAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS);

            _realseAction?.Invoke();

            await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);

            _ = DOFade.FadeOutAsync(0f);
        }

        public async void OnTouchResumeButton()
        {
            Color color = _backgroundDimmed.color;
            color.a = DIMMED_VALUE;
            _backgroundDimmed.color = color;

            _panelTransform.localScale = Vector2.one;

            InputBlock.Block(BLOCK_KEY);
            _ = _backgroundDimmed.DOFade(0, DISAPPEAR_TIME).SetUpdate(true);
            _ = _panelTransform.DOScale(new Vector2(0, 1), DISAPPEAR_TIME).SetEase(Ease.InBack).SetUpdate(true);
            await UniTask.Delay(TimeSpan.FromSeconds(DISAPPEAR_TIME), true);
            InputBlock.Release(BLOCK_KEY);

            _realseAction?.Invoke();
            PoolManager.Despawn(this);
        }

        public async void OnTouchSettingButton()
        {
            InputBlock.Block(BLOCK_KEY);
            await _settingPopupPrefab.InitializeAsync();
            InputBlock.Release(BLOCK_KEY);

            var popup = PoolManager.Spawn<SettingPopupUI>(_settingPopupPrefab, GameInstance.MainPopupFrame);
            popup.StretchRect();
            popup.Initialize().Forget();
        }

        private void HandleRelease()
        {
            _contentTransform.DetachChildren();
            Time.timeScale = 1f;
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
        }
    }
}
