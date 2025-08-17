using UnityEngine;
using System.Collections.Generic;
using DadVSMe.Background;
using System;

namespace DadVSMe.Background
{
    [Serializable]
    public class BackgroundTrailerGroup
    {
        public BackgroundLayerInfo layerInfo;
        public BackgroundTrailer trailer;

        public void Initialize(Collider2D boundary, BackgroundResourceReleaser releaser)
        {
            trailer.Initialize(layerInfo, boundary);
            releaser.HandleRegisterThemeData(layerInfo.themeDataArr);
        }
    }

    public class BackgroundSetup : MonoBehaviour
    {
        [SerializeField] private BackgroundTrailerGroup _backTrailerGroup;
        [SerializeField] private BackgroundTrailerGroup _middleTrailerGroup;
        [SerializeField] private BackgroundTrailerGroup _frontTrailerGroup;
        [Space(10)]
        [SerializeField] Collider2D _boundary;

        private BackgroundResourceReleaser _releaser;

        private void Start()
        {
            _releaser = new BackgroundResourceReleaser();

            _backTrailerGroup.Initialize(_boundary, _releaser);
            _middleTrailerGroup.Initialize(_boundary, _releaser);
            _frontTrailerGroup.Initialize(_boundary, _releaser);

            _backTrailerGroup.trailer.onChangedTheme += _releaser.HandleChangedNewLayerTheme;
            _middleTrailerGroup.trailer.onChangedTheme += _releaser.HandleChangedNewLayerTheme;
            _frontTrailerGroup.trailer.onChangedTheme += _releaser.HandleChangedNewLayerTheme;
        }
    }
}
