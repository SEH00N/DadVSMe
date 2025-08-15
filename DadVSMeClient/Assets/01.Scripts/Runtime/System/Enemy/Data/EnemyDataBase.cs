using UnityEngine;
using DadVSMe.Entities;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "CreateSO/Entity/Enemy/DataBase")]
    public class EnemyDataBase : ScriptableObject, IEntityData
    {
    }
}
