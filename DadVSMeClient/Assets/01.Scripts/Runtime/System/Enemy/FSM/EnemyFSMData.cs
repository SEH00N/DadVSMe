using UnityEngine;
using DadVSMe.Players;
using H00N.AI;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyFSMData : IAIData
    {
        public float patrolMinRange = 10f;
        public float patrolMaxRange = 30f;

        private Player player = null;
        public Player Player
        {
            get {
                if(player == null)
                {
                    if(GameInstance.GameCycle != null)
                        player = GameInstance.GameCycle.MainPlayer;
                }

                return player;
            }
        }

        public IAIData Initialize()
        {
            player = null;

            return this;
        }
    }
}