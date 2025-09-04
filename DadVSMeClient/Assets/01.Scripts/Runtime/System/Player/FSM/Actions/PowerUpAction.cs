using Cysharp.Threading.Tasks;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class PowerUpAction : FSMAction
    {
        [SerializeField] private AddressableAsset<AudioClip> sound = null;

        private Player owner;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            sound.InitializeAsync().Forget();
            owner = brain.GetComponent<Player>();
        }

        public override void EnterState()
        {
            base.EnterState();

            _ = new PlaySound(sound);
            owner.ActiveAnger();
        }
    }
}
