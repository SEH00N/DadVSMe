using System.Collections;
using UnityEngine;

namespace DadVSMe
{
    public class AutoActiveSkill : UnitSkill
    {
        private float cooltime;
        private Coroutine autoActiveCo;
        private WaitForSeconds wfs;

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            wfs = new WaitForSeconds(cooltime);

            autoActiveCo = ownerComponent.StartCoroutine(AutoActiveCo());
        }

        public override void Execute()
        {

        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            ownerComponent.StopCoroutine(autoActiveCo);
        }

        private IEnumerator AutoActiveCo()
        {
            while (true)
            {
                Execute();

                yield return wfs;
            }
        }
    }
}
