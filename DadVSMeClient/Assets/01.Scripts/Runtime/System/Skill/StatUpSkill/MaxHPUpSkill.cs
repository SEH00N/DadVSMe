using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class MaxHPUpSkill : StatUpSkill
    {
        public MaxHPUpSkill() : base()
        {
            StatUpRate = 30f;
        }

        public override void Execute()
        {
            base.Execute();

            UnitHealth health = ownerComponent.GetComponent<UnitHealth>();

            health.MaxHP = health.MaxHP + (int)StatUpAmount(); //임의 수식. 나중에 테이블 만들어서 가져오든 해야할듯
        }
    }
}
