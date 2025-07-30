using H00N.AI;
using System.Collections.Generic;
using DadVSMe.Enemies;

namespace DadVSMe.Players.FSM
{
    public class PlayerFSMData : IAIData
    {
        public List<Enemy> enemies = new List<Enemy>();

        public float moveSpeed = 5f;
        public float dashSpeed = 10f;

        public bool isComboReading = false;
        public bool isComboFailed = false;

        public int grabAttackCount = 0;

        public IAIData Initialize()
        {
            return this;
        }
    }
}