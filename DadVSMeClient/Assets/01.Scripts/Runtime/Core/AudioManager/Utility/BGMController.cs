using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe
{
    public partial class BGMController : MonoBehaviour
    {
        [SerializeField] BGMPlayer aChannel = null;
        [SerializeField] BGMPlayer bChannel = null;

        private BGMAudioLibrary prevBGMLibrary = null;
        private BGMAudioLibrary currentBGMLibrary = null;
        private BGMAudioLibraryTable currentBGMTable = null;

        private readonly Dictionary<BGMAudioLibrary, BGMCacheData> bgmCache = new Dictionary<BGMAudioLibrary, BGMCacheData>();
        private bool transitioning = false;
        private Scheduler scheduler = null;

        private void Awake()
        {
            scheduler = new Scheduler(aChannel, bChannel);
        }

        public void PlayBGM(BGMAudioLibrary bgmLibrary, bool immediately, bool loadCache)
        {
            if (bgmLibrary == currentBGMLibrary)
                return;

            PauseBGMInternal(immediately);
            ChangeBGMList(bgmLibrary);
            StartCoroutine(PlayBGMRoutine(loadCache));
        }

        public void PauseBGM(bool immediately)
        {
            PauseBGMInternal(immediately);
            ChangeBGMList(null);
        }

        private void PauseBGMInternal(bool immediately)
        {
            if (immediately)
                scheduler.PauseChannel();
            else
                scheduler.ReschedulePlayer();

            StopAllCoroutines();
        }

        public void ResumeBGM(bool immediately)
        {
            PlayBGM(prevBGMLibrary, immediately, true);
        }

        public void ClearCache()
        {
            bgmCache.Clear();
        }

        private void ChangeBGMList(BGMAudioLibrary bgmLibrary)
        {
            if (currentBGMLibrary != null && currentBGMLibrary.cacheProgress)
            {
                if (bgmCache.ContainsKey(currentBGMLibrary) == false)
                    bgmCache.Add(currentBGMLibrary, new BGMCacheData());
                bgmCache[currentBGMLibrary].SaveCache(currentBGMTable, scheduler.CurrentBGMPlayer.Player.time);
            }

            prevBGMLibrary = currentBGMLibrary;
            currentBGMLibrary = bgmLibrary;
            transitioning = currentBGMLibrary?.transitioning ?? false;
        }

        private IEnumerator PlayBGMRoutine(bool loadCache)
        {
            if (currentBGMLibrary == null)
                yield break;

            if(loadCache)
                yield return PlayCachedBGMRoutine(); // cache 된 브금 먼저 실행

            int bgmIndex = 0;
            while (true)
            {
                if (bgmIndex == 0)
                    currentBGMLibrary.ShuffleBGMList(); // 셔플 

                int pickLoopCount = 0;
                do
                {
                    // 모든 BGM이 null이라면 브금 루틴 종료
                    pickLoopCount++;
                    if (pickLoopCount > currentBGMLibrary.BGMCount)
                    {
                        PauseBGM(true);
                        yield break;
                    }

                    currentBGMTable = currentBGMLibrary.GetBGM(bgmIndex);
                    bgmIndex = (bgmIndex + 1) % currentBGMLibrary.BGMCount;
                }
                while (currentBGMTable == null || currentBGMTable.audioClip == null);

                scheduler.ToggleChannel();
                yield return PlayBGMRoutine(currentBGMTable);
            }
        }

        private IEnumerator PlayCachedBGMRoutine()
        {
            if (bgmCache.ContainsKey(currentBGMLibrary) == false)
                yield break;

            BGMCacheData cacheData = bgmCache[currentBGMLibrary];
            if (cacheData.IsValid == false)
                yield break;

            scheduler.ToggleChannel();

            float progress = cacheData.progress;
            currentBGMTable = cacheData.audioData;

            cacheData.ResetCache();

            yield return PlayBGMRoutine(currentBGMTable, progress);
        }

        private IEnumerator PlayBGMRoutine(BGMAudioLibraryTable bgmTable, float progress = 0f)
        {
            BGMPlayer player = scheduler.CurrentBGMPlayer;

            player.Initialize(bgmTable);
            player.PlayBGM(progress); // Play와 Pause는 AudioManager에서 관리함

            player.DoFadeIn(); // 실행과 동시에 fade in

            float delayDuration = bgmTable.length - progress;
            if (transitioning) // transitioning이 true라면 fade duration 까지만 놓고
                delayDuration -= bgmTable.fadeOutData.fadeDuration;

            // float time = 0f;
            // while(time < delayDuration)
            // {
            //     time += Time.unscaledDeltaTime;
            // }
            yield return new WaitForSecondsRealtime(delayDuration);

            player.DoFadeOut(); // fade out
            // 다음 루틴으로 넘어가서, 다음 BGM을 준비하고 전 fade out과 동시에 fade in
        }
    }
}