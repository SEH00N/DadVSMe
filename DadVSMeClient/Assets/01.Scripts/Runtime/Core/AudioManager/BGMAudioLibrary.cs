using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DadVSMe
{
    [Serializable]
    public class BGMAudioLibraryTable
    {
        public AudioClip audioClip;
        public float length;
        public FadeData fadeInData;
        public FadeData fadeOutData;
    }

    [CreateAssetMenu(menuName = "DadVSMe/Audio/BGMAudioLibrary")]
    public class BGMAudioLibrary : ScriptableObject
    { 
        [SerializeField] List<BGMAudioLibraryTable> audioLibraryTables = new List<BGMAudioLibraryTable>();

        public bool transitioning = true;
        public bool cacheProgress = true;
        public int BGMCount => audioLibraryTables?.Count ?? 0;

        public void ShuffleBGMList()
        {
            int n = audioLibraryTables.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);

                BGMAudioLibraryTable value = audioLibraryTables[k];
                audioLibraryTables[k] = audioLibraryTables[n];
                audioLibraryTables[n] = value;
            }
        }

        public BGMAudioLibraryTable GetBGM(int index)
        {
            return audioLibraryTables[index];
        }
    }
}