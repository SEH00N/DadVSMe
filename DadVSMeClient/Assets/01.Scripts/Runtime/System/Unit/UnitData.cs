using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "DadVSMe/Entity/UnitData")]
    public class UnitData : ScriptableObject
    {
        public int maxHP = 100;
    }
}