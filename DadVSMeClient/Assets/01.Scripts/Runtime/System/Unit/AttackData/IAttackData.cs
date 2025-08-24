
namespace DadVSMe.Entities
{
    public interface IAttackData
    {
        int Damage { get; }
        EAttackFeedback AttackFeedback { get; }

        public FeedbackData GetFeedbackData(EAttackAttribute attackAttribute); 
    }
}
