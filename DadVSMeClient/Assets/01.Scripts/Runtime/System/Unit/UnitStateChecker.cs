using System;
using System.Collections.Generic;
using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe.Entities
{
    [Serializable]
    public class UnitStateChecker
    {
        public bool checkHorizontalDistance = false;
        // [ConditionalField("checkHorizontalDistance", true, true)]
        public float attackHorizontalDistance = 3f;

        [Space(10f)]
        public bool checkVerticalDistance = false;
        // [ConditionalField("checkVerticalDistance", true, true)]
        public float attackVerticalDistance = 1f;

        [Space(10f)]
        public bool checkDirection = false;
        // [ConditionalField("checkDirection", true, true)]
        public bool directionMatch = false;

        [Space(10f)]
        public bool checkStatic = false;
        // [ConditionalField("checkStatic", true, true)]
        public bool staticCompare = false;

        [Space(10f)]
        public bool checkFloat = false;
        // [ConditionalField("checkFloat", true, true)]
        public bool floatCompare = false;

        [Space(10f)]
        public bool checkLie = false;
        // [ConditionalField("checkLie", true, true)]
        public bool lieCompare = false;

        [Space(10f)]
        public bool checkDie = false;
        // [ConditionalField("checkDie", true, true)]
        public bool dieCompare = false;

        public void Check(Unit unit, List<Unit> targets, Action<Unit> onMatch) => Check(unit, targets, null, onMatch);
        public void Check(Unit unit, List<Unit> targets, Transform pivot, Action<Unit> onMatch)
        {
            foreach(Unit target in targets)
            {
                if(Check(unit, target, pivot) == false)
                    continue;

                onMatch?.Invoke(target);
            }
        }

        public bool Check(Unit unit, Unit target, Transform pivot = null)
        {
            if(pivot == null)
                pivot = unit.transform;

            CheckUnitState checkUnitState = new CheckUnitState(unit, target, pivot)
                    .CheckDirection(checkDirection, directionMatch)
                    .CheckHorizontalDistance(checkHorizontalDistance, attackHorizontalDistance)
                    .CheckVerticalDistance(checkVerticalDistance, attackVerticalDistance)
                    .CheckStatic(checkStatic, staticCompare)
                    .CheckFloat(checkFloat, floatCompare)
                    .CheckLie(checkLie, lieCompare)
                    .CheckDie(checkDie, dieCompare);

            return checkUnitState.result;
        }
    }
}