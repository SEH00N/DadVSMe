using UnityEngine;
using H00N.AI;
using DadVSMe.Animals;

namespace DadVSMe.Enemies.FSM
{
    public class ShooterEnemyFSMData : IAIData
    {
        [HideInInspector] public Animal animal = null;

        public IAIData Initialize()
        {
            return this;
        }
    }
}