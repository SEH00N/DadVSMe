using DadVSMe.Entities;
using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class ReleaseGrabAction : FSMAction
    {
        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            if(fsmData.grabbedEntity == null)
                return;

            Entity grabbedEntity = fsmData.grabbedEntity;
            fsmData.grabbedEntity = null;

            grabbedEntity.transform.SetParent(null);
            (grabbedEntity as IGrabbable).Release(fsmData.player);
            if(grabbedEntity.TryGetComponent<FSMBrain>(out FSMBrain fsmBrain))
                fsmBrain.SetAsDefaultState();
        }
    }
}
