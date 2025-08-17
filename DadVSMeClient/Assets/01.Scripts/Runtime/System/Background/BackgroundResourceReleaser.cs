using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Background
{
    public class BackgroundResourceReleaser
    {
        private Dictionary<int, List<BackgroundThemeData>> _themeDataContainer;
        private Dictionary<int, int> _onUsingThemeDataContainer;

        public BackgroundResourceReleaser()
        {
            _themeDataContainer = new();
            _onUsingThemeDataContainer = new();
        }

        public void HandleRegisterThemeData(BackgroundThemeData[] themeDataArr)
        {
            foreach (var themeData in themeDataArr)
            {
                var themeIdx = themeData.themeIdx;

                if (_themeDataContainer.ContainsKey(themeIdx) == false)
                {
                    _themeDataContainer.Add(themeIdx, new List<BackgroundThemeData>());
                    _onUsingThemeDataContainer.Add(themeIdx, 0);
                }

                _themeDataContainer[themeIdx].Add(themeData);
                _onUsingThemeDataContainer[themeIdx]++;
            }
        }

        public void HandleChangedNewLayerTheme(int oldThemeIdx)
        {
            if (_themeDataContainer.ContainsKey(oldThemeIdx) == false)
            {
                Debug.LogError($"Not Register Theme ID : {oldThemeIdx}");
                return;
            }

            Debug.Log(oldThemeIdx);

            _onUsingThemeDataContainer[oldThemeIdx]--;

            if (_onUsingThemeDataContainer[oldThemeIdx] == 0)
            {
                ReleaseTheme(oldThemeIdx);
            }
        }

        private void ReleaseTheme(int oldThemeIdx)
        {
            Debug.Log($"Released Theme : {oldThemeIdx}");

            var list = _themeDataContainer[oldThemeIdx];

            foreach(var item in list)
            {
                item.ReleaseTheme();
            }

            _themeDataContainer.Remove(oldThemeIdx);
            _onUsingThemeDataContainer.Remove(oldThemeIdx);
        }
    }
}
