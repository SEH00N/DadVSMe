using System;
using Cysharp.Threading.Tasks;
using H00N.Resources;
using H00N.Resources.Addressables;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace DadVSMe.UI
{
    public class IntroUI : MonoBehaviour
    {
        private const float VIDEO_WAIT_TIME = -0.75f;
        private const float DELAY_TIME = 1.2f;

        [SerializeField] GameObject cutscenePanelObject = null;
        [SerializeField] VideoPlayer videoPlayer = null;
        [SerializeField] AddressableAsset<VideoClip> videoClipAsset = null;

        public async void OnTouchStartButton()
        {
            try 
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
                await UniTask.Delay(TimeSpan.FromSeconds(videoPlayer.length + VIDEO_WAIT_TIME), cancellationToken: destroyCancellationToken);

                ResourceManager.ReleaseResource(videoClipAsset);

                // Load Ongoing Scene
                await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
            }
            catch(OperationCanceledException) { }
        }

        public async void OnTouchSkipButton()
        {
            videoPlayer.time = videoPlayer.length + VIDEO_WAIT_TIME;
            ResourceManager.ReleaseResource(videoClipAsset);
            await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
