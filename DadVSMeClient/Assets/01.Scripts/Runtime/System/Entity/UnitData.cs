using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "CreateSO/Entity/UnitData")]
    public class UnitData : ScriptableObject
    {
        public int maxHP = 100;
    }
}