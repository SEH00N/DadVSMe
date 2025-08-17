using UnityEngine;
using DadVSMe.Entities;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "DadVSMe/Entity/Enemy/DataBase")]
    public class EnemyDataBase : ScriptableObject, IEntityData
    {
    }
}
