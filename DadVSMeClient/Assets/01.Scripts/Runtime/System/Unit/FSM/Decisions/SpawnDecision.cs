using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class SpawnDecision : FSMDecision
    {
        private bool isSpawn;

        public override bool MakeDecision()
        {
            if (isSpawn == false)
                isSpawn = true;
            else
                return false;

            return true;
        }
    }
}
