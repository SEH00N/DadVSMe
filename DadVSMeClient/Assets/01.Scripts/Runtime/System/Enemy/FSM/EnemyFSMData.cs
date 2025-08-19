using UnityEngine;
using DadVSMe.Players;
using H00N.AI;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyFSMData : IAIData
    {
        public float patrolMinRange = 10f;
        public float patrolMaxRange = 30f;

        [HideInInspector] public Player player = null;

        public IAIData Initialize()
        {
            return this;
        }
    }
}