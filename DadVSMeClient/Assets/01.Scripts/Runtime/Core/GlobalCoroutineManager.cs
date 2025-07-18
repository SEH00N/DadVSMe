using System.Collections;
using UnityEngine;

namespace DadVSMe
{
    public class GlobalCoroutineManager : MonoBehaviour, IManager
    {
        public static GlobalCoroutineManager Instance { get; private set; }

        public void Initialize()
        {
            Instance = this;
        }

        public void Release()
        {

        }

        public new Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return base.StartCoroutine(coroutine);
        }
    }
}