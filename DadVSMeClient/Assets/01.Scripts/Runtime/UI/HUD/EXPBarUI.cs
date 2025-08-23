using DadVSMe.Players;
using DadVSMe.Players.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe.UI.HUD
{
    public class EXPBarUI : MonoBehaviourUI
    {
        [SerializeField] Image fillImage = null;

        private PlayerFSMData playerFSMData = null;

        public void Initialize(Player player)
        {
            playerFSMData = player.FSMBrain.GetAIData<PlayerFSMData>();

            player.OnEXPChangedEvent += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if(playerFSMData == null)
                return;

            fillImage.fillAmount = Mathf.Clamp01(playerFSMData.currentExp / playerFSMData.levelUpExp);
        }
    }
}