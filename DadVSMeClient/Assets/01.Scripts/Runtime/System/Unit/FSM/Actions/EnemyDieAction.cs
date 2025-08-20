using Cysharp.Threading.Tasks;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class EnemyDieAction : DieAction
    {
        [SerializeField] AddressableAsset<Experience> expPrefab;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            expPrefab.InitializeAsync().Forget();
        }

        public override void EnterState()
        {
            base.EnterState();

            GameObject exp = PoolManager.Spawn(expPrefab);
            exp.transform.position = brain.transform.position;
        }
    }
}
