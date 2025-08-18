using UnityEngine;
using DadVSMe.Players;
using H00N.AI;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyFSMData : IAIData
    {
        [HideInInspector] public Player player = null;
        [HideInInspector] public float patrolMinRange = 10f;
        [HideInInspector] public float patrolMaxRange = 30f;

        public IAIData Initialize()
        {
            return this;
        }
    }
}