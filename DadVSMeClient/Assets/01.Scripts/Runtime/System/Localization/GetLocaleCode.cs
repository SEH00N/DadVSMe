namespace DadVSMe.Localizations
{
    public struct GetLocaleCode
    {
        public readonly string localeCode;

        public GetLocaleCode(ELocaleType localeType, string defaultLocaleCode = "en")
        {
            localeCode = localeType switch {
                ELocaleType.English => "en",
                ELocaleType.Korean => "ko",
                _ => defaultLocaleCode,
            };
        }
    }
}