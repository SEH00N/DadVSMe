using UnityEngine;
using H00N.Resources.Pools;
using DadVSMe.UI;
using System.Collections.Generic;
using System;

namespace DadVSMe.UI.Setting
{
    public class SettingPopupUI : PoolableBehaviourUI, SettingValueSlider.ICallback
    {
        [SerializeField] SettingValueSlider _masterValueSlider;
        [SerializeField] SettingValueSlider _bgmValueSlider;
        [SerializeField] SettingValueSlider _sfxValueSlider;

        private Dictionary<ESettingType, Action<float>> _valueChengeReactContainer;

        public void Initialize()
        {
            _valueChengeReactContainer = new Dictionary<ESettingType, Action<float>>();

            _valueChengeReactContainer.Add(ESettingType.Master, HandleChangedMasterValue);
            _valueChengeReactContainer.Add(ESettingType.BGM, HandleChangedBGMValue);
            _valueChengeReactContainer.Add(ESettingType.SFX, HandleChangedSFXValue);
        }

        public void OnValueChanged(ESettingType valueChengeType, float vlaue)
        {
            if(_valueChengeReactContainer.TryGetValue(valueChengeType, out var action))
            {
                action?.Invoke(vlaue);
            }
        }

        private void HandleChangedMasterValue(float value)
        {

        }

        private void HandleChangedBGMValue(float value)
        {
            
        }
        
        private void HandleChangedSFXValue(float value)
        {

        }
    }
}
