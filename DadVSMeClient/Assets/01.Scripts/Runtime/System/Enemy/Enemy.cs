using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class Enemy : Unit, IGrabbable
    {
        [SerializeField] FSMState grabState = null;

        // Debug
        private void Start()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        void IGrabbable.Grab(Entity performer)
        {
            fsmBrain.ChangeState(grabState);
        }
    }
}
