using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class FullAngerDecision : FSMDecision
    {
        private Unit owner;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            owner = brain.GetComponent<Unit>();
        }

        public override bool MakeDecision()
        {
            Player player = owner as Player;

            return player.CurrentAngerGauge >= player.MaxAngerGauge;
        }
    }
}
