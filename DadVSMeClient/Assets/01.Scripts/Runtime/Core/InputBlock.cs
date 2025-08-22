using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DadVSMe
{
    public static class InputBlock
    {
        private static Dictionary<string, CancellationTokenSource> blockedInput = null;

        private static EventSystem eventSystem = null;
        public static EventSystem EventSystem {
            get {
                if(eventSystem == null)
                    eventSystem = UnityEngine.Object.FindFirstObjectByType<EventSystem>();

                return eventSystem;
            }
        }

        static InputBlock()
        {
            blockedInput = new Dictionary<string, CancellationTokenSource>();
        }

        public static void Block(string key, float timeout = 5f)
        {
            EventSystem.enabled = false;
            BlockAsync(key, timeout);
        }

        public static void Release(string key)
        {
            if(blockedInput.TryGetValue(key, out var cancellationTokenSource))
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
            }

            blockedInput.Remove(key);
            if(blockedInput.Count == 0)
                EventSystem.enabled = true;
        }
        
        private static async void BlockAsync(string key, float timeout)
        {
            if(blockedInput.TryGetValue(key, out CancellationTokenSource cancellationTokenSource))
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
            }

            CancellationTokenSource newCancellationTokenSource = new CancellationTokenSource();

            try {
                blockedInput[key] = newCancellationTokenSource;

                await UniTask.Delay(TimeSpan.FromSeconds(timeout), cancellationToken: newCancellationTokenSource.Token);

                if(newCancellationTokenSource.IsCancellationRequested == false)
                    blockedInput.Remove(key);
            } 
            catch(OperationCanceledException) { }
            finally {
                newCancellationTokenSource?.Dispose();
            }
        }
    }
}
