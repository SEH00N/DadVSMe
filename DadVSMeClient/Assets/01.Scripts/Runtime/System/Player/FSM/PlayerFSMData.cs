using H00N.AI;
using UnityEngine;
using DadVSMe.Entities;

namespace DadVSMe.Players.FSM
{
    public class PlayerFSMData : IAIData
    {
        public Transform grabParent = null;
        public Transform grabPosition = null;
        public Transform throw1Position = null;
        public Transform throw2Position = null;

        [HideInInspector] public Entity grabbedEntity = null;

        [HideInInspector] public bool isComboReading = false;
        [HideInInspector] public bool isComboFailed = false;

        [HideInInspector] public int grabAttackCount = 0;

        public float maxAngerGauge;
        [HideInInspector] public float currentAngerGauge;
        public float angerTime;
        [HideInInspector] public bool isAnger;

        public IAIData Initialize()
        {
            return this;
        }
    }
}