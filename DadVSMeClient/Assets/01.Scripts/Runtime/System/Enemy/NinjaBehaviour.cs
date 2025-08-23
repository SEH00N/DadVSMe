using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class NinjaBehaviour : MonoBehaviour
    {
        [SerializeField] Unit unit = null;

        private UnitFSMData unitFSMData = null;
        private NinjaFSMData ninjaFSMData = null;
        private NinjaData ninjaData = null;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
        }

        private void Update()
        {
            if (unitFSMData.isDie || unitFSMData.isFloat || unitFSMData.isLie)
            {
                ninjaFSMData.jumpAttackTimer = ninjaData.jumpAttackCooltime;
                return;
            }

            if (ninjaFSMData.jumpAttackTimer > 0f)
                ninjaFSMData.jumpAttackTimer -= Time.deltaTime;
        }

        public void ResetButtTimer()
        {
            ninjaFSMData.jumpAttackTimer = ninjaData.jumpAttackCooltime;
        }

        private void InitializeInternal(IEntityData data)
        {
            if (data is NinjaData ninjaData == false)
                return;

            this.ninjaData = ninjaData;
            unitFSMData = unit.FSMBrain.GetAIData<UnitFSMData>();
            ninjaFSMData = unit.FSMBrain.GetAIData<NinjaFSMData>();
        }
    }
}
