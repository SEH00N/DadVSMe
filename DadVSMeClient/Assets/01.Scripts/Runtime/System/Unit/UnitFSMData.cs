using System.Collections.Generic;
using H00N.AI;

namespace DadVSMe.Entities
{
    public class UnitFSMData : IAIData
    {
        public Unit unit = null;
        public int enemyMaxCount = 5;
        public List<Unit> enemies = new List<Unit>();
        public IAttackData attackData = null;
        public int forwardDirection = 1;
        public float attackRange = 3f;

        public IAIData Initialize()
        {
            return this;
        }
    }
}