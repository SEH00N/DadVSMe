using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace DadVSMe
{
    public static class DOFade
    {
        private const float INPUT_BLOCK_TIMEOUT = 60f;
        private const string INPUT_BLOCK_KEY = "DOFade";

        public static async UniTask FadeInAsync(float duration = GameDefine.DEFAULT_FADE_DURATION)
        {
            GameInstance.FadeImage.raycastTarget = true;
            InputBlock.Block(INPUT_BLOCK_KEY, INPUT_BLOCK_TIMEOUT);

            GameInstance.FadeImage.DOKill();
            GameInstance.FadeImage.color = new Color(GameInstance.FadeImage.color.r, GameInstance.FadeImage.color.g, GameInstance.FadeImage.color.b, 0f);
            await GameInstance.FadeImage.DOFade(1f, duration).SetEase(Ease.OutCubic);
        }

        public static async UniTask FadeOutAsync(float duration = GameDefine.DEFAULT_FADE_DURATION)
        {
            GameInstance.FadeImage.DOKill();
            GameInstance.FadeImage.color = new Color(GameInstance.FadeImage.color.r, GameInstance.FadeImage.color.g, GameInstance.FadeImage.color.b, 1f);
            await GameInstance.FadeImage.DOFade(0f, duration).SetEase(Ease.InCubic);

            GameInstance.FadeImage.raycastTarget = false;
            InputBlock.Release(INPUT_BLOCK_KEY);
        }
    }
}