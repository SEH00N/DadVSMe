using UnityEngine;

namespace DadVSMe.Localizations
{
    public struct GetSystemLocale
    {
        public readonly ELocaleType localeType;

        public GetSystemLocale(ELocaleType defaultLocaleType)
        {
            SystemLanguage language = Application.systemLanguage;
            localeType = language switch {
                SystemLanguage.English => ELocaleType.English,
                SystemLanguage.Korean => ELocaleType.Korean,
                _ => defaultLocaleType,
            };
        }
    }
}