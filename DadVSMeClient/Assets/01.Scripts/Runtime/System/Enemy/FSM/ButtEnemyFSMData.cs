using H00N.AI;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class ButtEnemyFSMData : IAIData
    {
        [HideInInspector] public float buttTimer = 0f;

        public IAIData Initialize()
        {
            buttTimer = 0f;
            return this;
        }
    }
}