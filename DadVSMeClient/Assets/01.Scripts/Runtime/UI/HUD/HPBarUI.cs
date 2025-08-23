using DadVSMe.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe.UI.HUD
{
    public class HPBarUI : MonoBehaviourUI
    {
        [SerializeField] Image fillImage = null;

        private UnitStatData unitStatData = null;
        private UnitHealth unitHealth = null;

        public void Initialize(Unit unit)
        {
            unitStatData = unit.FSMBrain.GetAIData<UnitStatData>();
            unitHealth = unit.GetComponent<UnitHealth>();

            unitHealth.OnHPChangedEvent += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if(unitHealth == null || unitStatData == null)
                return;

            fillImage.fillAmount = Mathf.Clamp01(unitHealth.CurrentHP / unitStatData[EUnitStat.MaxHp].FinalValue);
        }
    }
}