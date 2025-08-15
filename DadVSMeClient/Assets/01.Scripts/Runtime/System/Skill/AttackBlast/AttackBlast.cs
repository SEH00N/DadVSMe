using System.Collections;
using DadVSMe.Entities;
using H00N.Resources;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AttackBlast : Entity
    {
        private UnitMovement movement;
        private PoolReference poolReference;

        private GameObject instigator;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float lifeTime;
        private WaitForSeconds wfs;

        void Awake()
        {
            Initialize(null);
        }

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);

            movement = GetComponent<UnitMovement>();
            poolReference = GetComponent<PoolReference>();
            wfs = new WaitForSeconds(lifeTime);
        }

        public void SetInstigator(GameObject instigator)
        {
            this.instigator = instigator;
        }

        public void Lunch(Vector3 direction)
        {
            movement.SetActive(true);
            movement.SetMovementVelocity(direction * moveSpeed);

            StartCoroutine(Despawn());
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == instigator)
                return;
            if (collision.gameObject.CompareTag("Enemy") == false)
                return;

            

            PoolManager.Despawn(poolReference);
        }

        private IEnumerator Despawn()
        {
            yield return wfs;

            PoolManager.Despawn(poolReference);
        }
    }
}
