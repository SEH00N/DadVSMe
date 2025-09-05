using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace DadVSMe.UI
{
    public class IntroUI : MonoBehaviour
    {
        private const float WAIT_TIME = 29f;
        private const float DELAY_TIME = 1.2f;

        [SerializeField] GameObject cutscenePanelObject = null;
        [SerializeField] GameObject videoHolderObject = null;
        [SerializeField] VideoPlayer videoPlayer = null;
        [SerializeField] AddressableAsset<BGMAudioLibrary> bgmAudioLibrary = null;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // private void Start()
        // {
        //     // Load Resources
        //     new LoadResourceByLabel().LoadAsync(GameDefine.ADDRESSABLES_LABEL_GAME_ASSETS).Forget();
        // }

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
                await bgmAudioLibrary.InitializeAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                cutscenePanelObject.SetActive(true);
                videoHolderObject.SetActive(false);

                // videoPlayer.clip = videoClipAsset;
                videoPlayer.url = Path.Combine(Application.streamingAssetsPath, "IntroVideo.mp4");
                videoPlayer.Prepare();
                await UniTask.WaitUntil(() => videoPlayer.isPrepared, cancellationToken: cancellationTokenSource.Token);

                // FadeOut
                await DOFade.FadeOutAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                // Play video
                videoHolderObject.SetActive(true);
                videoPlayer.Play();
                AudioManager.Instance.PlayBGM(bgmAudioLibrary, loadCache: false);

                // Wait for video to finish
                await UniTask.Delay(TimeSpan.FromSeconds(WAIT_TIME), cancellationToken: cancellationTokenSource.Token);

                if(cancellationTokenSource.IsCancellationRequested)
                    return;

                // Load Ongoing Scene
                await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
            }
            catch(OperationCanceledException) { }
            finally {
                // ResourceManager.ReleaseResource(videoClipAsset);
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        public async void OnTouchSkipButton()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;

            videoPlayer.Stop();
            await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
