using H00N.AI;
using System.Collections.Generic;
using DadVSMe.Enemies;
using UnityEngine;
using DadVSMe.Entities;

namespace DadVSMe.Players.FSM
{
    public class PlayerFSMData : IAIData
    {
        public Entity player = null;

        public Transform grabPosition = null;
        public Entity grabbedEntity = null;

        public List<Enemy> enemies = new List<Enemy>();

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