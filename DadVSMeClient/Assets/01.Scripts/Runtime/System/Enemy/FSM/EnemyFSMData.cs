using DadVSMe.Players;
using H00N.AI;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyFSMData : IAIData
    {
        public Player player = null;

        public IAIData Initialize()
        {
            return this;
        }
    }
}