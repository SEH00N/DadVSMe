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

        public async void OnTouchStartButton()
        {
            try 
            {                
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

                Debug.Log("1111111");

                // Holding
                await UniTask.Delay(TimeSpan.FromSeconds(DELAY_TIME), cancellationToken: cancellationTokenSource.Token);

                Debug.Log("2222222");

                // FadeIn
                await DOFade.FadeInAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                Debug.Log("3333333");

                // Load video clip
                await bgmAudioLibrary.InitializeAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                Debug.Log("4444444");

                cutscenePanelObject.SetActive(true);
                videoHolderObject.SetActive(false);

                Debug.Log("5555555");

                // videoPlayer.clip = videoClipAsset;
                videoPlayer.url = Path.Combine(Application.streamingAssetsPath, "IntroVideo.mp4");
                videoPlayer.Prepare();
                await UniTask.WaitUntil(() => videoPlayer.isPrepared, cancellationToken: cancellationTokenSource.Token);

                Debug.Log("6666666");

                // FadeOut
                await DOFade.FadeOutAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                Debug.Log("7777777");

                // Play video
                videoHolderObject.SetActive(true);
                videoPlayer.Play();
                AudioManager.Instance.PlayBGM(bgmAudioLibrary, loadCache: false);

                Debug.Log("8888888");

                // Wait for video to finish
                await UniTask.Delay(TimeSpan.FromSeconds(WAIT_TIME), cancellationToken: cancellationTokenSource.Token);

                Debug.Log("9999999");

                if(cancellationTokenSource.IsCancellationRequested)
                    return;

                Debug.Log("10101010");

                // Load Ongoing Scene
                await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);

                Debug.Log("11111111");
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
