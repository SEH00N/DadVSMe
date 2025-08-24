using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class BullyEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] Unit unit = null;
        private BullyEnemyFSMData fsmData;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
        }

        private void Update()
        {
            fsmData.currnetAttack1Cooltime = Mathf.Max(fsmData.currnetAttack1Cooltime - Time.deltaTime, 0f);
            fsmData.currnetAttack2Cooltime = Mathf.Max(fsmData.currnetAttack2Cooltime - Time.deltaTime, 0f);
        }

        public void ResetAttack1Timer()
        {
            fsmData.currnetAttack1Cooltime = fsmData.Attack1Cooltime;
        }

        public void ResetAttack2Timer()
        {
            fsmData.currnetAttack2Cooltime = fsmData.Attack2Cooltime;
        }


        private void InitializeInternal(IEntityData data)
        {
            fsmData = unit.FSMBrain.GetAIData<BullyEnemyFSMData>();
        }
    }
}
