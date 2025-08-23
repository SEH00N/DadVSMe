using System;
using UnityEngine;

namespace DadVSMe.Localizations
{
    public struct LocalizedString
    {
        private const string LOCALIZATION_TABLE_NAME = "LocCommonTable";

        public string localizedString;

        public LocalizedString(UnityEngine.Localization.LocalizedString localizedString)
        {
            this.localizedString = localizedString.GetLocalizedString();
        }

        public LocalizedString(string key)
        {
            localizedString = key;

            try
            {
                UnityEngine.Localization.LocalizedString localizedString = new() { TableReference = LOCALIZATION_TABLE_NAME, TableEntryReference = key };
                this.localizedString = localizedString.GetLocalizedString();
            }
            catch (Exception err)
            {
                Debug.LogError($"[Localization] GetLocalizedString Exception: {err}");
            }
        }

        public static implicit operator string(LocalizedString getLocalizedString) => getLocalizedString.localizedString;
        public override readonly string ToString() => localizedString;
    }
}