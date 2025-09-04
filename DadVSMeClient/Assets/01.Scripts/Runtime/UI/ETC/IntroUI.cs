using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using H00N.Resources;
using H00N.Resources.Addressables;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace DadVSMe.UI
{
    public class IntroUI : MonoBehaviour
    {
        private const float VIDEO_WAIT_TIME = -0.75f;
        private const float DELAY_TIME = 1.2f;

        [SerializeField] GameObject cutscenePanelObject = null;
        [SerializeField] GameObject videoHolderObject = null;
        [SerializeField] VideoPlayer videoPlayer = null;
        [SerializeField] AddressableAsset<BGMAudioLibrary> bgmAudioLibrary = null;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private Dictionary<string, string> appSettings = new Dictionary<string, string>() {
            { "IntroVideo", "IntroVideo.mp4" }
        };

        private void Awake()
        {
            Debug.Log(JsonConvert.SerializeObject(appSettings));
        }

        #if UNITY_WEBGL && !UNITY_EDITOR
        private async void Start()
        {
            appSettings = await LoadAppSettings();
        }

        private async UniTask<Dictionary<string, string>> LoadAppSettings()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "appsettings.json");
        
            UnityWebRequest www = UnityWebRequest.Get(filePath);
        
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                return new Dictionary<string, string>() {
                    { "IntroVideo", "IntroVideo.mp4" }
                };

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text);
        }
        #endif

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
                videoPlayer.url = Path.Combine(Application.streamingAssetsPath, appSettings["IntroVideo"]);
                videoPlayer.Prepare();
                await UniTask.WaitUntil(() => videoPlayer.isPrepared, cancellationToken: cancellationTokenSource.Token);

                // FadeOut
                await DOFade.FadeOutAsync().AttachExternalCancellation(cancellationTokenSource.Token);

                // Play video
                videoHolderObject.SetActive(true);
                videoPlayer.Play();
                AudioManager.Instance.PlayBGM(bgmAudioLibrary, loadCache: false);

                // Wait for video to finish
                await UniTask.Delay(TimeSpan.FromSeconds(videoPlayer.length + VIDEO_WAIT_TIME), cancellationToken: cancellationTokenSource.Token);

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

            videoPlayer.time = videoPlayer.length + VIDEO_WAIT_TIME;
            await SceneManager.TryLoadSceneAsync(GameDefine.TITLE_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}
