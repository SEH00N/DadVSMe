using System;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Background
{
    [Serializable]
    public class BackgroundLayerInfo
    {
        public BackgroundThemeData[] themeDataArr;
        public Transform parentTransform;
        public Transform startTransform;
    }
}
