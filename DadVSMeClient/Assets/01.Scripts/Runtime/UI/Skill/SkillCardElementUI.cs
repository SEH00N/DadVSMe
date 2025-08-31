using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.Localizations;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DadVSMe.UI.Skills
{
    public class SkillCardElementUI : PoolableBehaviourUI<SkillCardElementUI.ICallback>
    {
        public interface ICallback : IUICallback
        {
            void OnSelectCard(SkillData skillData);
        }

        // Animation Value
        private const int APPEAR_DELAY_FRAME = 8;
        private const float EXAGGERATE_TIME = 0.3f;
        private const float EXAGGERATE_SCALE_VALUE = 1.1f;
        private const float EXAGGERATE_MAINTAIN_TIME = 0.1f;
        private const float EXAGGERATE_EXIT_TIME = 0.4f;
        private const int EXAGGERATE_ROTATION_VALUE = 10;

        private const string BLOCK_KEY = "SkillCardElementUI";

        public bool IsSelected { get; private set; } = false;

        [SerializeField] Image skillIcon = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text descText = null;
        [SerializeField] List<GameObject> levelObjectList = null;
        [SerializeField] EventTrigger hoverTrigger = null;

        private SkillData skillData = null;

        public async UniTask Initialize(SkillData skillData, int currentLevel, ICallback callback, int index)
        {
            base.Initialize(callback);
            this.skillData = skillData;

            new SetSprite(skillIcon, skillData.skillIcon);
            _ = new SetLocalizedString(skillData.skillName, nameText);
            _ = new SetLocalizedString(currentLevel == 0 ? skillData.skillDesc : skillData.skillLevelUpDesc, descText);

            for(int i = 0; i < levelObjectList.Count; i++)
                levelObjectList[i].SetActive(i < currentLevel);

            // UI Animation
            IsSelected = false;
            hoverTrigger.enabled = false;

            transform.localScale = Vector3.zero;
            transform.rotation = Quaternion.identity;

            await UniTask.DelayFrame(APPEAR_DELAY_FRAME * index);
            await PlayAppearAnimation();

            hoverTrigger.enabled = true;
        }

        private async UniTask PlayAppearAnimation()
        {
            await UniTask.DelayFrame(1);

            transform.DOKill();

            _ = transform.DOScale(Vector3.one * EXAGGERATE_SCALE_VALUE, EXAGGERATE_TIME)
                         .SetUpdate(true);
            _ = transform.DORotate(new Vector3(0, 0, EXAGGERATE_ROTATION_VALUE), EXAGGERATE_TIME)
                         .SetUpdate(true);

            await UniTask.Delay(TimeSpan.FromSeconds(EXAGGERATE_TIME + EXAGGERATE_MAINTAIN_TIME), true, cancellationToken: destroyCancellationToken);

            _ = transform.DOScale(Vector3.one, EXAGGERATE_EXIT_TIME)
                         .SetEase(Ease.OutBack)
                         .SetUpdate(true);
            await transform.DORotate(-Vector3.one * EXAGGERATE_SCALE_VALUE / 2, EXAGGERATE_EXIT_TIME / 2)
                         .SetEase(Ease.OutBack)
                         .SetUpdate(true);

            _ = transform.DORotate(Vector3.zero, EXAGGERATE_EXIT_TIME / 2)
                         .SetEase(Ease.OutBack)
                         .SetUpdate(true);
        }

        public async void OnTouchThis()
        {
            for(int i = 0; i < levelObjectList.Count; i++)
            {
                if(levelObjectList[i].activeSelf)
                    continue;

                levelObjectList[i].SetActive(true);
                break;
            }

            IsSelected = true;

            callback.OnSelectCard(skillData);

            InputBlock.Block(BLOCK_KEY);
            await UniTask.Delay(TimeSpan.FromSeconds(EXAGGERATE_TIME * 3), true, cancellationToken: destroyCancellationToken);
            InputBlock.Release(BLOCK_KEY);
        }

        public void OnHoverThis()
        {
            transform.DOKill();
            transform.DOScale(Vector2.one * EXAGGERATE_SCALE_VALUE, EXAGGERATE_TIME / 2).SetUpdate(true);
        }

        public void OutHoverThis()
        {
            transform.DOKill();
            transform.DOScale(Vector2.one, EXAGGERATE_TIME / 2).SetEase(Ease.OutBack).SetUpdate(true);
        }

        public async UniTask PlayReleasAnimation()
        {
            transform.DOKill();

            await transform.DOScale(Vector2.zero, EXAGGERATE_TIME).SetEase(Ease.InBack).SetUpdate(true);
        }
    }
}