using UnityEngine;

namespace DadVSMe
{
    public static class GameSettings
    {
        private static float masterVolume = float.MinValue;
        public static float MasterVolume {
            get {
                if(masterVolume == int.MinValue)
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
                if(bgmVolume == int.MinValue)
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
                if(sfxVolume == int.MinValue)
                    sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

                return sfxVolume;
            }

            set {
                sfxVolume = value;
                PlayerPrefs.SetFloat("SFXVolume", value);
            }
        }
    }
}