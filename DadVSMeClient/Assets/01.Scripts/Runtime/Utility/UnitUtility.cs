using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public struct CheckUnitState
    {
        public bool result;

        private readonly Unit owner;
        private readonly Unit target;
        private readonly Transform pivot;

        public CheckUnitState(Unit owner, Unit target, Transform pivot)
        {
            result = true;

            this.owner = owner;
            this.target = target;
            this.pivot = pivot;
        }

        public CheckUnitState CheckDirection(bool check, bool directionMatch)
        {
            if(check == false || result == false)
                return this;

            int forwardDirection = owner.FSMBrain.GetAIData<UnitFSMData>().forwardDirection;
            float targetDirection = target.transform.position.x - pivot.position.x;
            result = directionMatch == (Mathf.Sign(targetDirection) == Mathf.Sign(forwardDirection));

            return this;
        }

        public CheckUnitState CheckHorizontalDistance(bool check, float distance)
        {
            if(check == false || result == false)
                return this;

            result = Mathf.Abs(target.transform.position.x - pivot.position.x) <= distance;

            return this;
        }

        public CheckUnitState CheckVerticalDistance(bool check, float distance)
        {
            if(check == false || result == false)
                return this;

            result = Mathf.Abs(target.transform.position.y - pivot.position.y) <= distance;

            return this;
        }

        public CheckUnitState CheckStatic(bool check, bool staticCompare)
        {
            if(check == false || result == false)
                return this;

            result = target.StaticEntity == staticCompare;

            return this;
        }

        public CheckUnitState CheckFloat(bool check, bool floatCompare)
        {
            if(check == false || result == false)
                return this;

            result = target.FSMBrain.GetAIData<UnitFSMData>().isFloat == floatCompare;

            return this;
        }

        public CheckUnitState CheckLie(bool check, bool lieCompare)
        {
            if(check == false || result == false)
                return this;

            result = target.FSMBrain.GetAIData<UnitFSMData>().isLie == lieCompare;

            return this;
        }

        public CheckUnitState CheckDie(bool check, bool dieCompare)
        {
            if(check == false || result == false)
                return this;

            result = target.FSMBrain.GetAIData<UnitFSMData>().isDie == dieCompare;

            return this;
        }
    }
}