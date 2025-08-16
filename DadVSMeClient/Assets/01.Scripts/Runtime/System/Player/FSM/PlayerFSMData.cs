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
        public Entity grabbedEntity = null;

        public float moveSpeed = 5f;
        public float dashSpeed = 10f;

        public bool isComboReading = false;
        public bool isComboFailed = false;

        public int grabAttackCount = 0;

        public IAIData Initialize()
        {
            return this;
        }
    }
}