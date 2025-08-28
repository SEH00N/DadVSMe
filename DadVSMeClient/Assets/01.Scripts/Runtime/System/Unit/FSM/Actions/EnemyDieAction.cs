using Cysharp.Threading.Tasks;
using DadVSMe.Entities.FSM;
using DadVSMe.Items;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class EnemyDieAction : DieAction
    {
        [SerializeField] AddressableAsset<Experience> expPrefab;
        [SerializeField] AddressableAsset<HealPack> healPackPrefab;
        [SerializeField, Range(0f, 1f)] float healPackProbability;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            expPrefab.InitializeAsync().Forget();
            healPackPrefab.InitializeAsync().Forget();
        }

        public override void EnterState()
        {
            base.EnterState();

            Item item = SpawnItemByProbability();
            item.transform.position = new Vector3(brain.transform.position.x, brain.transform.position.y, 0);
        }

        private Item SpawnItemByProbability()
        {
            float random = Random.value;
            PoolReference prefab = random < healPackProbability ? healPackPrefab.Asset : expPrefab.Asset;
            return PoolManager.Spawn<Item>(prefab);
        }
    }
}
