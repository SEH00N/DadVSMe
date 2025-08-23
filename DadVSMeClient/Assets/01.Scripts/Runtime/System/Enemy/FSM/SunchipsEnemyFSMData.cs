using H00N.AI;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsEnemyFSMData : IAIData
    {
        [HideInInspector] public int shootCount;
        [HideInInspector] public float shootCooltime;
        [HideInInspector] public float buttTimer = 0f;

        public IAIData Initialize()
        {
            return this;
        }
    }
}
