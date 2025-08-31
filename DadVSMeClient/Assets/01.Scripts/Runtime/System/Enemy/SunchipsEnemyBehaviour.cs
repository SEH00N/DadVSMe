using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class SunchipsEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] Unit unit = null;
        [SerializeField] PoolReference poolReference = null;
        public PoolReference PoolReference => poolReference;

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
        }

        private void InitializeInternal(IEntityData data)
        {
            if (data is SunchipsEnemyData sunchipsEnemyData == false)
                return;

            this.sunchipsEnemyData = sunchipsEnemyData;
            fsmData = unit.FSMBrain.GetAIData<SunchipsEnemyFSMData>();
        }
        
        public void ResetButtTimer()
        {
            fsmData.buttTimer = sunchipsEnemyData.buttCooltime;
        }
    }
}
