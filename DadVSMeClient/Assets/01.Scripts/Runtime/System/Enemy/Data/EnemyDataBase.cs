using UnityEngine;
using DadVSMe.Entities;

namespace DadVSMe.Enemies
{
    public abstract class EnemyDataBase : ScriptableObject, IEntityData 
    { 
        public float patrolMinRange = 10f;
        public float patrolMaxRange = 30f;
    }
}
