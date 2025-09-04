using DadVSMe.Players;
using DadVSMe.Players.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe.UI.HUD
{
    public class RageBarUI : MonoBehaviourUI
    {
        [SerializeField] Image fillImage = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Color defaultBackgroundColor = Color.white;
        [SerializeField] Color rageBackgroundColor = Color.red;

        private PlayerFSMData playerFSMData = null;
        private float currentAngerGauge = 0f;
        private bool isRage = false;

        public void Initialize(Player player)
        {
            playerFSMData = player.FSMBrain.GetAIData<PlayerFSMData>();
            UpdateFillImage();
            UpdateBackgroundImage();
        }

        private void Update()
        {
            if(playerFSMData == null)
                return;

            if(currentAngerGauge != playerFSMData.currentAngerGauge)
                UpdateFillImage();
            
            if(isRage != playerFSMData.isAnger)
                UpdateBackgroundImage();
        }

        private void UpdateFillImage()
        {            
            currentAngerGauge = playerFSMData.currentAngerGauge;
            fillImage.fillAmount = Mathf.Clamp01(playerFSMData.currentAngerGauge / playerFSMData.maxAngerGauge);
        }

        private void UpdateBackgroundImage()
        {
            isRage = playerFSMData.isAnger;
            backgroundImage.color = isRage ? rageBackgroundColor : defaultBackgroundColor;
        }
    }
}