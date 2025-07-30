using DadVSMe.Enemies;
using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class GrabAction : FSMAction
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

            if(fsmData.grabbedEntity != null)
                return;

            Enemy enemy = fsmData.enemies[0];
            if(enemy.TryGetComponent<Entity>(out Entity entity) == false)
            {
                brain.SetAsDefaultState();
                return;
            }

            fsmData.grabbedEntity = entity;
            fsmData.grabbedEntity.transform.SetParent(fsmData.grabPosition);
            fsmData.grabbedEntity.transform.localPosition = Vector3.zero;
        }
    }
}
