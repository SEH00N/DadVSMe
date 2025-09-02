using System;
using System.Collections.Generic;
using H00N.AI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DadVSMe.Entities
{
    public class UnitFSMData : IAIData
    {
        public int enemyMaxCount = 5;
        public Unit unit = null;

        [HideInInspector] public List<Unit> enemies = new List<Unit>();
        [HideInInspector] public IAttackData attackData = null;
        [HideInInspector] public int forwardDirection = 1;
        [HideInInspector] public float groundPositionY = 0f;
        [HideInInspector] public UnitCollisionData collisionData = new UnitCollisionData();
        [HideInInspector] public bool isFloat = false;
        [HideInInspector] public bool isLie = false;
        [HideInInspector] public bool isDie = false;
        [HideInInspector] public HashSet<Object> holders = null;
        [HideInInspector] public EAttackAttribute hitAttribute;
        [HideInInspector] public EAttackAttribute attackAttribute;
        public Action<Unit> OnBowlingEvent;

        public IAIData Initialize()
        {
            holders = new HashSet<Object>();
            attackAttribute = EAttackAttribute.Normal;
            return this;
        }
    }
}