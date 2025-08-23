using H00N.AI;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class NinjaFSMData : IAIData
    {
        [HideInInspector] public float jumpAttackTimer = 0f;

        public IAIData Initialize()
        {
            return this;
        }
    }
}
