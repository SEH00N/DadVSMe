using H00N.AI;
using UnityEngine;
using DadVSMe.Entities;
using System;
using H00N.AI.FSM;

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
        public Action<Entity> onGrabbedEntityChanged;

        public float maxAngerGauge;
        [HideInInspector] public float currentAngerGauge;
        public float angerTime;
        [HideInInspector] public bool isAnger;

        public float baseLevelUpXP = 3f;
        public float levelUpRatio = 1.7f;
        [HideInInspector] public float levelUpExp;
        [HideInInspector] public float currentExp;
        [HideInInspector] public int currentLevel = 1;

        public IAIData Initialize()
        {
            levelUpExp = baseLevelUpXP;
            currentLevel = 1;
            currentExp = 0;
            currentAngerGauge = 0;
            isAnger = false;

            return this;
        }
    }
}