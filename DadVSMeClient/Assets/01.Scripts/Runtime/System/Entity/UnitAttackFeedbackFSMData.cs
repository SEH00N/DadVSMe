using H00N.AI;

namespace DadVSMe.Entities
{
    public class UnitAttackFeedbackFSMData : IAIData
    {
        public float feedbackValue = 0f;

        public IAIData Initialize()
        {
            return this;
        }
    }
}