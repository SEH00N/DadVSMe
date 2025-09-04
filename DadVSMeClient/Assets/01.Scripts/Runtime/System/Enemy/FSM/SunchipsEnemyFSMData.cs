using DadVSMe.Entities;
using H00N.AI;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsEnemyFSMData : IAIData
    {
        [HideInInspector] public int shootCount;
        [HideInInspector] public float shootCooltime;
        [HideInInspector] public float buttTimer = 0f;
        [HideInInspector] public Unit frenzyTarget;
        public float frenzyCoolTime;
        [HideInInspector] public float currentFrenzyCooltime;

        public IAIData Initialize()
        {
            shootCount = 0;
            shootCooltime = 0f;
            buttTimer = 0f;
            frenzyTarget = null;
            currentFrenzyCooltime = 0f;

            return this;
        }
    }
}
