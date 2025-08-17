using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class SimpleAttackAction : AttackActionBase
    {
        
        [SerializeField] AttackDataBase attackData = null;

        [Space(10f)]
        [SerializeField] bool checkHorizontalRange = true;
        [SerializeField] float attackHorizontalRange = 3f;
        [SerializeField] bool checkVerticalRange = true;
        [SerializeField] float attackVerticalRange = 1f;

        [Space(10f)]
        [SerializeField] bool checkFloat = false;
        [SerializeField] bool isFloat = false;

        [Space(10f)]
        [SerializeField] bool checkLie = false;
        [SerializeField] bool isLie = false;

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            int forwardDirection = unitFSMData.forwardDirection;
            unitFSMData.enemies.ForEach(enemy =>
            {
                float targetDirection = enemy.transform.position.x - transform.position.x;
                if (Mathf.Sign(targetDirection) != Mathf.Sign(forwardDirection))
                    return;

                if (checkHorizontalRange)
                {
                    if (Mathf.Abs(targetDirection) > attackHorizontalRange)
                        return;
                }

                if (checkVerticalRange)
                {
                    if (Mathf.Abs(enemy.transform.position.y - transform.position.y) > attackVerticalRange)
                        return;
                }

                if (checkFloat)
                {
                    if (enemy.FSMBrain.GetAIData<UnitFSMData>().isFloat != isFloat)
                        return;
                }

                if (checkLie)
                {
                    if (enemy.FSMBrain.GetAIData<UnitFSMData>().isLie != isLie)
                        return;
                }

                AttackToTarget(enemy, attackData);
            });
        }
    }
}