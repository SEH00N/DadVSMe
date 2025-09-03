using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe.UI
{
    public class EndingUI : MonoBehaviour
    {
        [SerializeField] List<Image> imageList = null;
        [SerializeField] RectTransform creditTransform = null;
        [SerializeField] Image retryUIFadeImage = null;
        [SerializeField] RectTransform playAgainButtonTransform = null;

        private void Start()
        {
            // start animation
            PlayAnimation();
        }

        private async void PlayAnimation()
        {
            foreach (Image image in imageList)
            {
                image.gameObject.SetActive(true);
                image.color = Color.white;
            }
            
            await UniTask.WaitForSeconds(1f);

            foreach (Image image in imageList)
            {
                await UniTask.WaitForSeconds(4f);
                await image.DOFade(0f, 2f).SetEase(Ease.OutCubic);
            }

            await UniTask.WaitForSeconds(1f);

            // await creditTransform.DOAnchorPosY(1060f, 2f).SetEase(Ease.InOutSine);
            // await UniTask.WaitForSeconds(3f);
            // await creditTransform.DOAnchorPosY(1935f, 2f).SetEase(Ease.InOutSine);
            // await UniTask.WaitForSeconds(3f);
            // await creditTransform.DOAnchorPosY(2855, 2f).SetEase(Ease.InOutSine);
            // await UniTask.WaitForSeconds(3f);
            // await creditTransform.DOAnchorPosY(3870, 2f).SetEase(Ease.InOutSine);
            // await UniTask.WaitForSeconds(3f);
            // await creditTransform.DOAnchorPosY(5020, 2f).SetEase(Ease.InOutSine);
            // await UniTask.WaitForSeconds(3f);
            // await creditTransform.DOAnchorPosY(6100, 2f).SetEase(Ease.InOutSine);

            await creditTransform.DOAnchorPosY(100, 3f).SetEase(Ease.InCubic);
            await creditTransform.DOAnchorPosY(6100, 50f).SetEase(Ease.Linear);

            await UniTask.WaitForSeconds(1f);

            _ = retryUIFadeImage.DOFade(0f, 0.75f).SetEase(Ease.OutCubic);
            await UniTask.WaitForSeconds(0.5f);
            await playAgainButtonTransform.DOAnchorPosY(0f, 0.65f).SetEase(Ease.OutCubic);
        }

        public void OnTouchPlayAgainButton()
        {
            _ = new StartGame().StartGameAsync(loadResources: false);
        }
    }
}