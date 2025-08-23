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
        private SunchipsEnemyData sunchipsEnemyData = null;
        private SunchipsEnemyFSMData fsmData;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
        }

        private void Update()
        {
            fsmData.shootCooltime = Mathf.Max(fsmData.shootCooltime - Time.deltaTime, 0f);
            fsmData.currentFrenzyCooltime = Mathf.Max(fsmData.currentFrenzyCooltime - Time.deltaTime, 0f);

            if (unitFSMData.isDie || unitFSMData.isFloat || unitFSMData.isLie)
            {
                fsmData.buttTimer = sunchipsEnemyData.buttCooltime;
                return;
            }

            if (fsmData.buttTimer > 0f)
                fsmData.buttTimer -= Time.deltaTime;
        }

        private void InitializeInternal(IEntityData data)
        {
            if (data is SunchipsEnemyData sunchipsEnemyData == false)
                return;

            this.sunchipsEnemyData = sunchipsEnemyData;
            unitFSMData = unit.FSMBrain.GetAIData<UnitFSMData>();
            enemyFSMData = unit.FSMBrain.GetAIData<EnemyFSMData>();
            fsmData = unit.FSMBrain.GetAIData<SunchipsEnemyFSMData>();
        }
        
        public void ResetButtTimer()
        {
            fsmData.buttTimer = sunchipsEnemyData.buttCooltime;
        }
    }
}
