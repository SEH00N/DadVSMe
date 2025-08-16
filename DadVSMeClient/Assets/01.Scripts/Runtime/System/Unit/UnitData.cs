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

    public class UnitData : ScriptableObject
    {
        [SerializeField]
        private UnitStatContainer stat;
        public UnitStatContainer Stat => stat;
    }
}