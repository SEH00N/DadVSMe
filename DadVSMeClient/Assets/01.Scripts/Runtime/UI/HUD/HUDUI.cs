using Cysharp.Threading.Tasks;
using DadVSMe.UI.Skills;
using UnityEngine;

namespace DadVSMe.UI.HUD
{
    public class HUDUI : MonoBehaviourUI
    {
        [SerializeField] SkillInfoUI skillInfoUI = null;

        // Debug
        private async void Start()
        {
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
            Initialize();
        }

        public void Initialize()
        {
            skillInfoUI.Initialize(GameInstance.MainPlayer.GetComponent<UnitSkillComponent>());
        }
    }
}