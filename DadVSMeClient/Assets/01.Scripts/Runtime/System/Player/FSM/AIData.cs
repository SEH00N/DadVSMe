using H00N.AI;

namespace DadVSMe.Players.FSM
{
    public class AIData : IAIData
    {
        public bool isComboReading = false;
        public bool isComboFailed = false;

        public IAIData Initialize()
        {
            return this;
        }
    }
}