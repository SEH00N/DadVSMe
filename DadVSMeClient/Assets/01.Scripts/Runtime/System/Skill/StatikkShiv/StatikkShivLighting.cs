using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class StatikkShivLighting : MonoBehaviour
    {
        [SerializeField]
        private AttackDataBase attackData;

        private PoolReference poolReference;

        void Awake()
        {
            poolReference = GetComponent<PoolReference>();
        }

        public void Active(Unit instigator, Unit target)
        {
            if (instigator == null)
                return;
            if (target == null)
                return;

            transform.position = target.transform.position;
            if (target.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                targetHealth.Attack(instigator, attackData);
            }
        }

        public void Despawn()
        {
            PoolManager.Despawn(poolReference);
        }
    }
}
