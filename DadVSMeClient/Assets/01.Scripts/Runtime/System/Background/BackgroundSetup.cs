using UnityEngine;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace DadVSMe.Background
{
    [Serializable]
    public class BackgroundTrailerGroup
    {
        public BackgroundLayerInfo layerInfo;
        public BackgroundTrailer trailer;

        public void Initialize(Collider2D deadLineboundary, Collider2D backgroundLoadBoundary, Transform cameraTrm, BackgroundResourceReleaser releaser)
        {
            trailer.Initialize(layerInfo, cameraTrm, deadLineboundary, backgroundLoadBoundary);
            releaser.HandleRegisterThemeData(layerInfo.themeDataArr);
        }
    }

    public class BackgroundSetup : MonoBehaviour
    {
        [SerializeField] Transform _cameraTransform;
        [SerializeField] Collider2D _deadLineBoundary;
        [SerializeField] Collider2D _backgroundLoadBoundary;
        [Space(20)]
        [SerializeField] private BackgroundTrailerGroup _backTrailerGroup;
        [SerializeField] private BackgroundTrailerGroup _middleTrailerGroup;
        [SerializeField] private BackgroundTrailerGroup _bottomTrailerGroup;

        private void Start()
        {
            var releaser = new BackgroundResourceReleaser();

            InitializeBackgroundTrailerGroup(_backTrailerGroup, releaser);
            InitializeBackgroundTrailerGroup(_middleTrailerGroup, releaser);
            InitializeBackgroundTrailerGroup(_bottomTrailerGroup, releaser);
        }

        private void InitializeBackgroundTrailerGroup(BackgroundTrailerGroup group, BackgroundResourceReleaser releaser)
        {
            group.Initialize(_deadLineBoundary, _backgroundLoadBoundary, _cameraTransform, releaser);
            group.trailer.onDespawnedBackground += releaser.HandleChangedNewLayerTheme;
            group.trailer.Run().Forget();
        }
    }
}
