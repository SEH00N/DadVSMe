using System.Collections.Generic;
using H00N.AI;
using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(menuName = "DadVSMe/Entity/UnitStatData")]
    public class UnitStatData : ScriptableObject, IAIData
    {
        [SerializeField] List<UnitStat> stats = null;        
        public UnitStat this[EUnitStat statType] => stats.Find(x => x.StatType == statType);

        public IAIData Initialize()
        {
            UnitStatData instance = Instantiate(this);
            foreach (EUnitStat statType in EnumHelper.GetValues<EUnitStat>())
                instance[statType]?.CalcFinalValue();

            return instance;
        }
    }
}