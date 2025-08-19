using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class ButtEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] Unit unit = null;

        private UnitFSMData unitFSMData = null;
        private ButtEnemyFSMData buttEnemyFSMData = null;
        private ButtEnemyData buttEnemyData = null;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
        }

        private void Update()
        {
            if(unitFSMData.isDie || unitFSMData.isFloat || unitFSMData.isLie)
            {
                buttEnemyFSMData.buttTimer = buttEnemyData.buttCooltime;
                return;
            }

            if(buttEnemyFSMData.buttTimer > 0f)
                buttEnemyFSMData.buttTimer -= Time.deltaTime;
        }

        public void ResetButtTimer()
        {
            buttEnemyFSMData.buttTimer = buttEnemyData.buttCooltime;
        }

        private void InitializeInternal(IEntityData data)
        {
            if(data is ButtEnemyData buttEnemyData == false)
                return;

            this.buttEnemyData = buttEnemyData;
            unitFSMData = unit.FSMBrain.GetAIData<UnitFSMData>();
            buttEnemyFSMData = unit.FSMBrain.GetAIData<ButtEnemyFSMData>();
        }
    }
}
