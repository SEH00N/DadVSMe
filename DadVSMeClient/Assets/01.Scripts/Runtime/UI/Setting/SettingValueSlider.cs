using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe.UI.Setting
{
    public class SettingValueSlider : PoolableBehaviourUI<SettingValueSlider.ICallback>
    {
        [SerializeField] ESettingType _valueChangeType;
        [SerializeField] Slider _slider;

        public interface ICallback : IUICallback
        {
            void OnValueChanged(ESettingType valueChengeType, float vlaue);
        }

        public void Initialize(ESettingType valueChengeType, ICallback callback)
        {
            base.Initialize(callback);
        }

        public void OnChangedValue(float value)
        {
            callback.OnValueChanged(_valueChangeType, value);
        }
    }
}
