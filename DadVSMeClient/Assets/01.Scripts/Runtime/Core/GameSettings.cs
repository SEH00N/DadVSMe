using DadVSMe.Localizations;
using UnityEngine;

namespace DadVSMe
{
    public static class GameSettings
    {
        #if UNITY_EDITOR
        public static class Editor
        {
            public static bool SHOULD_PLAY_GAME_START_DIRECTING {
                get => UnityEditor.EditorPrefs.GetInt("ShouldPlayGameStartDirecting", 1) == 1;
                set => UnityEditor.EditorPrefs.SetInt("ShouldPlayGameStartDirecting", value ? 1 : 0);
            }
        }
        #endif

        private static float masterVolume = float.MinValue;
        public static float MasterVolume {
            get {
                if(masterVolume == float.MinValue)
                    masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

                return masterVolume;
            }

            set {
                masterVolume = value;
                PlayerPrefs.SetFloat("MasterVolume", value);
            }
        }

        private static float bgmVolume = float.MinValue;
        public static float BGMVolume {
            get {
                if(bgmVolume == float.MinValue)
                    bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);

                return bgmVolume;
            }

            set {
                bgmVolume = value;
                PlayerPrefs.SetFloat("BGMVolume", value);
            }
        }

        private static float sfxVolume = float.MinValue;
        public static float SFXVolume {
            get {
                if(sfxVolume == float.MinValue)
                    sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

                return sfxVolume;
            }

            set {
                sfxVolume = value;
                PlayerPrefs.SetFloat("SFXVolume", value);
            }
        }

        private static ELocaleType localeType = ELocaleType.None;
        public static ELocaleType LocaleType {
            get {
                if(localeType == ELocaleType.None)
                    localeType = (ELocaleType)PlayerPrefs.GetInt("LocaleType", (int)ELocaleType.English);

                return localeType;
            }

            set {
                localeType = value;
                PlayerPrefs.SetInt("LocaleType", (int)value);
            }
        }
    }
}