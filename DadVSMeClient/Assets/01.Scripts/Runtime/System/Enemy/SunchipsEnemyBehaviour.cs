using Cysharp.Threading.Tasks;
using DadVSMe.Animals;
using DadVSMe.Enemies;
using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class SunchipsEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] Unit unit = null;
        [SerializeField] Transform animalFollowTarget = null;

        [SerializeField] PoolReference poolReference = null;
        public PoolReference PoolReference => poolReference;

        private UnitFSMData unitFSMData = null;
        private EnemyFSMData enemyFSMData = null;
        private SunchipsEnemyData shooterEnemyData = null;

        private SunchipsEnemyFSMData fsmData;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
            fsmData = GetComponent<FSMBrain>().GetAIData<SunchipsEnemyFSMData>();
        }

        private void Update()
        {
            fsmData.shootCooltime = Mathf.Max(fsmData.shootCooltime - Time.deltaTime, 0f);
        }
        
        private void InitializeInternal(IEntityData data)
        {
            if(data is SunchipsEnemyData shooterEnemyData == false)
                return;

            this.shooterEnemyData = shooterEnemyData;
            unitFSMData = unit.FSMBrain.GetAIData<UnitFSMData>();
            enemyFSMData = unit.FSMBrain.GetAIData<EnemyFSMData>();
        }

        public void OnSpawned() { }
        public void OnDespawn()
        {
        }
    }
}
