using System;
using System.Collections.Generic;
using ShibaInspector.Collections;
using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "DadVSMe/Entity/UnitData")]

    [System.Serializable]
    public class UnitStatContainer
    {
        [SerializeField]
        private List<UnitStat> stats;
        
        public UnitStat this[EUnitStat statType]
        {
            get
            {
                return stats.Find(x => x.StatType == statType);
            }
        }
    }

    public class UnitData : ScriptableObject, IEntityData
    {
        [SerializeField]
        private UnitStatContainer stat;
        public UnitStatContainer Stat => stat;

        public float angerTime;
        public bool isAnger;

        [HideInInspector]
        public EAttackAttribute attackAttribute;

        public void Initiallize()
        {
            foreach (EUnitStat statType in Enum.GetValues(typeof(EUnitStat)))
            {
                if (stat[statType] != null)
                {
                    stat[statType].CalcFinalValue();
                }
            }

            attackAttribute = EAttackAttribute.Normal;
        }
    }
}