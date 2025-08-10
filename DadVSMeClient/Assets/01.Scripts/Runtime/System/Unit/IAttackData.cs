
namespace DadVSMe.Entities
{
    public interface IAttackData
    {
        int Damage { get; }
        EAttackFeedback AttackFeedback { get; }
        float AttackFeedbackValue { get; }
    }
}
