using UnityEngine;
using System;

namespace DadVSMe.Background
{
    [Serializable]
    public class BackgroundTrailerGroup
    {
        public BackgroundLayerInfo layerInfo;
        public BackgroundTrailer trailer;

        public void Initialize(Collider2D boundary, Transform cameraTrm, BackgroundResourceReleaser releaser)
        {
            trailer.Initialize(layerInfo, cameraTrm, boundary);
            releaser.HandleRegisterThemeData(layerInfo.themeDataArr);
        }
    }

    public class BackgroundSetup : MonoBehaviour
    {
        [SerializeField] private BackgroundTrailerGroup _backTrailerGroup;
        [SerializeField] private BackgroundTrailerGroup _middleTrailerGroup;
        [SerializeField] private BackgroundTrailerGroup _frontTrailerGroup;
        [Space(10)]
        [SerializeField] Transform _cameraTransform;
        [SerializeField] Collider2D _boundary;

        private BackgroundResourceReleaser _releaser;

        private void Start()
        {
            _releaser = new BackgroundResourceReleaser();

            _backTrailerGroup.Initialize(_boundary, _cameraTransform, _releaser);
            //_middleTrailerGroup.Initialize(_boundary, _cameraTransform, _releaser);
            //_frontTrailerGroup.Initialize(_boundary, _cameraTransform, _releaser);

            _backTrailerGroup.trailer.onDespawnedBackground += _releaser.HandleChangedNewLayerTheme;
            //_middleTrailerGroup.trailer.onChangedTheme += _releaser.HandleChangedNewLayerTheme;
            //_frontTrailerGroup.trailer.onChangedTheme += _releaser.HandleChangedNewLayerTheme;

            _backTrailerGroup.trailer.Run();
        }
    }
}
