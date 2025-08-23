using DadVSMe.Enemies;
using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class GrabbableWeightDecision : FSMDecision
    {
        public float grabbableWeight;

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override bool MakeDecision()
        {
            if (unitFSMData.enemies.Count == 0)
                return false;

            foreach (Unit unit in unitFSMData.enemies)
            {
                if (unit == null)
                    continue;

                Enemy enemy = unit as Enemy;
                if (enemy == null)
                    continue;

                if (enemy.Weight > grabbableWeight)
                    continue;

                return true;
            }

            return false;
        }
    }
}
