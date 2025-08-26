using Cysharp.Threading.Tasks;
using DadVSMe.GameCycles;
using DadVSMe.Players;
using DadVSMe.UI.Skills;
using UnityEngine;

namespace DadVSMe.UI.HUD
{
    public class HUDUI : MonoBehaviourUI
    {
        [SerializeField] HPBarUI hpBarUI = null;
        [SerializeField] RageBarUI rageBarUI = null;
        [SerializeField] EXPBarUI expBarUI = null;
        [SerializeField] SkillInfoUI skillInfoUI = null;
        [SerializeField] GameProgressUI gameProgressUI = null;
        [SerializeField] AnimationUI goPopupUI = null;

        public void Initialize()
        {
            GameCycle gameCycle = GameInstance.GameCycle;
            Player player = gameCycle.MainPlayer;
            hpBarUI.Initialize(player);
            rageBarUI.Initialize(player);
            expBarUI.Initialize(player);
            skillInfoUI.Initialize(player.GetComponent<UnitSkillComponent>());
            gameProgressUI.Initialize(gameCycle.StartLine, gameCycle.EndLine, gameCycle.Deadline.transform, player.transform);
        }

        public void ActiveGoPopupUI()
        {
            goPopupUI.gameObject.SetActive(true);
        }
    }
}