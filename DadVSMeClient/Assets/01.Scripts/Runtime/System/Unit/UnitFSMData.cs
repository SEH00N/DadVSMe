using System.Collections.Generic;
using H00N.AI;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class UnitFSMData : IAIData
    {
        public Unit unit = null;
        public int enemyMaxCount = 5;
        public List<Unit> enemies = new List<Unit>();
        public IAttackData attackData = null;
        public int forwardDirection = 1;
        public float groundPositionY = 0f;
        public UnitCollisionData collisionData = new UnitCollisionData();
        public bool isFloat = false;
        public bool isLie = false;
        public EAttackAttribute hitAttribute;

        public IAIData Initialize()
        {
            return this;
        }
    }
}