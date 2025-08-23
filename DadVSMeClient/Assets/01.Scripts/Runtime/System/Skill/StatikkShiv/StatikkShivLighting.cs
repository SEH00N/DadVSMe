using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class StatikkShivLighting : MonoBehaviour
    {
        [SerializeField]
        private AttackDataBase attackData;

        private PoolReference poolReference;
        public LineRenderer lineRenderer;
        public LineRendererAnimator lineRendererAnimator;
        public LineRenderer outlineRenderer;
        public LineRendererAnimator outlineRendererAnimator;

        void Awake()
        {
            poolReference = GetComponent<PoolReference>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRendererAnimator = GetComponent<LineRendererAnimator>();
        }

        public async void Active(Unit instigator, int attackNum, float attackRadius)
        {
            if (instigator == null)
                return;

            Vector3[] points = new Vector3[attackNum + 1];
            List<UnitHealth> targets = new();
            points[0] = instigator.transform.position;
            int count = 0;

            for (int i = 0; i < attackNum; i++)
            {
                if (count >= attackNum)
                    break;

                Collider2D[] cols = Physics2D.OverlapCircleAll(points[i], attackRadius);
                if (cols.Length == 0)
                    break;

                bool findTarget = false;
                foreach (var col in cols)
                {
                    if (col.gameObject == instigator.gameObject)
                        continue;

                    if (col.TryGetComponent<UnitHealth>(out UnitHealth health))
                    {
                        if (targets.Contains(health))
                            continue;

                        count++;
                        points[count] = health.transform.position;
                        targets.Add(health);

                        health.Attack(instigator, attackData);
                        findTarget = true;

                        if (count >= attackNum)
                            break;
                    }
                }

                if (count >= attackNum)
                    break;

                if (findTarget == false)
                    break;
            }

            lineRenderer.positionCount = targets.Count + 1;
            lineRenderer.SetPositions(points);
            lineRendererAnimator.StartAnimation(.05f, .1f);

            outlineRenderer.positionCount = targets.Count + 1;
            outlineRenderer.SetPositions(points);
            outlineRendererAnimator.StartAnimation(.05f, .1f);

            await UniTask.Delay(TimeSpan.FromSeconds(3));

            Despawn();
        }

        public void Despawn()
        {
            PoolManager.Despawn(poolReference);
            lineRenderer.positionCount = 0;
            outlineRenderer.positionCount = 0;
        }
    }
}
