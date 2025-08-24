using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class HasSkillDecision : FSMDecision
    {
        public SkillType skill;
        UnitSkillComponent skillComp;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            skillComp = brain.GetComponent<UnitSkillComponent>();
        }

        public override bool MakeDecision()
        {
            if (skillComp == null)
                return false;

            return skillComp.SkillContainer.ContainsKey(skill);
        }
    }
}
