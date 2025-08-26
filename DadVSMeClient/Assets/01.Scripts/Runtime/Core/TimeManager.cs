using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DadVSMe
{
    public static class TimeManager
    {
        private const float TICK = 0.02f;

        private static CancellationTokenSource cancellationTokenSource = null;

        private static Queue<Action> timeScalingQueue = null;

        public static void Initialize()
        {
            timeScalingQueue = new Queue<Action>();
        }

        public static void Release()
        {
            if(cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }

            if(timeScalingQueue != null)
            {
                timeScalingQueue.Clear();
                timeScalingQueue = null;
            }
        }

        public static void SetTimeScale(float timeScale, bool force) => SetTimeScale(timeScale, force, 0f);
        public static async void SetTimeScale(float timeScale, bool force, float duration)
        {
            if(cancellationTokenSource != null)
            {
                if(force == false)
                {
                    timeScalingQueue.Enqueue(() => SetTimeScale(timeScale, force, duration));
                }
                else
                {
                    timeScalingQueue.Clear();
                    cancellationTokenSource?.Cancel();
                    cancellationTokenSource?.Dispose();
                    cancellationTokenSource = null;
                }
            }

            if(duration <= 0f)
            {
                Time.timeScale = timeScale;
                FlushQueue();
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();

            await SetTimeScaleInternal(timeScale, duration, cancellationTokenSource.Token);

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;

            FlushQueue();
        }

        private static async UniTask SetTimeScaleInternal(float timeScale, float duration, CancellationToken cancellationToken)
        {
            try {
                float timer = 0f;
                while(timer < duration)
                {
                    timer += TICK;
                    Time.timeScale = Mathf.Lerp(GameDefine.DEFAULT_TIME_SCALE, timeScale, timer / duration);
                    await UniTask.WaitForSeconds(TICK, cancellationToken: cancellationToken);
                }

                Time.timeScale = timeScale;
            }
            catch(OperationCanceledException) { }
        }

        private static void FlushQueue()
        {
            if(timeScalingQueue.Count <= 0)
                return;

            Action action = timeScalingQueue.Dequeue();
            action?.Invoke();
        }
    }
}