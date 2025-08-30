using System;
using System.Threading;
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

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public async void OnTouchStartButton()
        {
            try 
            {                
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

                // Holding
                await UniTask.Delay(TimeSpan.FromSeconds(DELAY_TIME), cancellationToken: cancellationTokenSource.Token);

                // FadeIn
                await DOFade.FadeInAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                // Load video clip
                await videoClipAsset.InitializeAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                cutscenePanelObject.SetActive(true);
                videoPlayer.clip = videoClipAsset;
                videoPlayer.Prepare();
                await UniTask.WaitUntil(() => videoPlayer.isPrepared, cancellationToken: cancellationTokenSource.Token);

                // FadeOut
                await DOFade.FadeOutAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                // Play video
                videoPlayer.Play();

                // Wait for video to finish
                await UniTask.Delay(TimeSpan.FromSeconds(videoPlayer.length + VIDEO_WAIT_TIME), cancellationToken: cancellationTokenSource.Token);

                if(cancellationTokenSource.IsCancellationRequested)
                    return;

                // Load Ongoing Scene
                await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
            }
            catch(OperationCanceledException) { }
            finally {
                ResourceManager.ReleaseResource(videoClipAsset);
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        public async void OnTouchSkipButton()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;

            videoPlayer.time = videoPlayer.length + VIDEO_WAIT_TIME;
            await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
