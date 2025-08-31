using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DadVSMe.Localizations
{
    public struct SetLocalizedString
    {
        private const string LOCALIZATION_TABLE_NAME = "LocCommonTable";

        public SetLocalizedString(UnityEngine.Localization.LocalizedString localizedString, TMP_Text text)
        {
            SetLocalizedStringAsync(localizedString, text);
        }

        public SetLocalizedString(string key, TMP_Text text)
        {
            try
            {
                UnityEngine.Localization.LocalizedString localizedString = new() { TableReference = LOCALIZATION_TABLE_NAME, TableEntryReference = key };
                SetLocalizedStringAsync(localizedString, text);
            }
            catch (Exception err)
            {
                Debug.LogError($"[Localization] GetLocalizedString Exception: {err}");
            }
        }

        private async void SetLocalizedStringAsync(UnityEngine.Localization.LocalizedString localizedString, TMP_Text text)
        {
            try
            {
                string result = await localizedString.GetLocalizedStringAsync();
                text.text = result;
            }
            catch (Exception err)
            {
                Debug.LogError($"[Localization] GetLocalizedString Exception: {err}");
            }
        }
    }

    // public struct LocalizedString
    // {
    //     private const string LOCALIZATION_TABLE_NAME = "LocCommonTable";

    //     public string localizedString;

    //     public LocalizedString(UnityEngine.Localization.LocalizedString localizedString)
    //     {
    //         this.localizedString = localizedString.GetLocalizedString();
    //     }

    //     public LocalizedString(string key)
    //     {
    //         localizedString = key;

    //         try
    //         {
    //             UnityEngine.Localization.LocalizedString localizedString = new() { TableReference = LOCALIZATION_TABLE_NAME, TableEntryReference = key };
    //             this.localizedString = localizedString.GetLocalizedString();
    //         }
    //         catch (Exception err)
    //         {
    //             Debug.LogError($"[Localization] GetLocalizedString Exception: {err}");
    //         }
    //     }

    //     public static implicit operator string(LocalizedString getLocalizedString) => getLocalizedString.localizedString;
    //     public override readonly string ToString() => localizedString;
    // }
}