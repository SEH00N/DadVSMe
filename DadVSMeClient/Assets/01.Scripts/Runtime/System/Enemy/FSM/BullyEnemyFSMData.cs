using H00N.AI;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class BullyEnemyFSMData : IAIData
    {
        [HideInInspector] public float currnetAttack1Cooltime;
        [HideInInspector] public float currnetAttack2Cooltime;
        public float Attack1Cooltime;
        public float Attack2Cooltime;

        public IAIData Initialize()
        {
            return this;
        }
    }
}
