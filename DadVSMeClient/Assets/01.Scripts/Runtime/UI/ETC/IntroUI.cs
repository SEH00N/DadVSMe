using System;
using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace DadVSMe.UI
{
    public class IntroUI : MonoBehaviour
    {
        private const float DELAY_TIME = 1.2f;

        [SerializeField] GameObject cutscenePanelObject = null;
        [SerializeField] VideoPlayer videoPlayer = null;
        [SerializeField] AddressableAsset<VideoClip> videoClipAsset = null;

        public async void OnTouchStartButton()
        {
            // Holding
            await UniTask.Delay(TimeSpan.FromSeconds(DELAY_TIME));

            // FadeIn
            await DOFade.FadeInAsync();

            // Load video clip
            await videoClipAsset.InitializeAsync();

            cutscenePanelObject.SetActive(true);
            videoPlayer.clip = videoClipAsset;
            videoPlayer.Prepare();
            await UniTask.WaitUntil(() => videoPlayer.isPrepared);

            // FadeOut
            await DOFade.FadeOutAsync();

            // Play video
            videoPlayer.Play();

            // Wait for video to finish
            await UniTask.Delay(TimeSpan.FromSeconds(videoPlayer.length));

            // Load Ongoing Scene
            await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
