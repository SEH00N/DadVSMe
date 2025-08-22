using Cysharp.Threading.Tasks;
using UnityLocalizationSettings = UnityEngine.Localization.Settings.LocalizationSettings;

namespace DadVSMe.Localizations
{
    public static class LocalizationSettings
    {
        public static async UniTask InitializeAsync()
        {
            await UnityLocalizationSettings.InitializationOperation;
            SetLocale(GameSettings.LocaleType);
        }

        public static void SetLocale(ELocaleType localeType)
        {
            UnityLocalizationSettings.SelectedLocale = UnityLocalizationSettings.AvailableLocales.GetLocale(new GetLocaleCode(localeType).localeCode);
            GameSettings.LocaleType = localeType;
        }
    }
}