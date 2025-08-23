using H00N.AI;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsEnemyFSMData : IAIData
    {
        [HideInInspector]
        public int shootCount;
        [HideInInspector]
        public float shootCooltime;

        public IAIData Initialize()
        {
            return this;
        }
    }
}
