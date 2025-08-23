using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DG.Tweening;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class Bomb : MonoBehaviour
    {
        public AddressableAsset<PoolableEffect> effectRef;
        public AddressableAsset<AudioClip> soundRef;
        public AttackDataBase attackData;
        public float attackRadius;
        public float jumpPower;
        public float jumpDuration;

        Unit owner;
        PoolReference poolReference;

        void Awake()
        {
            effectRef.InitializeAsync().Forget();
            soundRef.InitializeAsync().Forget();
            poolReference = GetComponent<PoolReference>();
        }

        public void JumpToTarget(Transform target, Unit owner)
        {
            if (target == null)
                return;

            this.owner = owner;
            transform.DOJump(target.position, jumpPower, 1, jumpDuration).OnComplete(() => Explosion());
        }

        private void Explosion()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, attackRadius);
            foreach (var col in cols)
            {
                if(col.gameObject == owner.gameObject)
                    continue;

                if (col.TryGetComponent<UnitHealth>(out UnitHealth health))
                {
                    health.Attack(owner, attackData);
                }
            }

            _ = new PlayEffect(effectRef, transform.position, 1);
            _ = new PlaySound(soundRef);

            PoolManager.Despawn(poolReference);
        }
    }
}
