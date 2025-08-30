using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.Core.Cam;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class StatikkShivLighting : MonoBehaviour
    {
        private PoolReference poolReference;
        public LineRenderer lineRenderer;
        public LineRendererAnimator lineRendererAnimator;
        public LineRenderer outlineRenderer;
        public LineRendererAnimator outlineRendererAnimator;

        public float appearTime;
        public float disappearTime;

        void Awake()
        {
            poolReference = GetComponent<PoolReference>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRendererAnimator = GetComponent<LineRendererAnimator>();
        }

        public async void Active(Unit instigator, int attackNum, float attackRadius, IAttackData attackData, IAttackFeedbackDataContainer feedbackDataContainer)
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

            AttackAsync(targets, instigator, attackData, feedbackDataContainer);

            points[0] = instigator.transform.position;
            lineRenderer.positionCount = targets.Count + 1;
            for (int i = 0; i <= targets.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
            lineRendererAnimator.StartAnimationPerSegment(appearTime, disappearTime);

            // outlineRenderer.positionCount = targets.Count + 1;
            // outlineRenderer.SetPositions(points);
            // outlineRendererAnimator.StartAnimationPerSegment(.02f, .03f);

            await UniTask.Delay(TimeSpan.FromSeconds(3));

            Despawn();
        }

        private async void AttackAsync(List<UnitHealth> targets, Unit instigator, IAttackData attackData, IAttackFeedbackDataContainer feedbackDataContainer)
        {
            if(targets.Count > 2)
            {
                BackgroundFilterCameraHandle handle = CameraManager.CreateCameraHandle<BackgroundFilterCameraHandle, BackgroundFilterCameraHandleParameter>(out BackgroundFilterCameraHandleParameter param);
                param.time = 0.25f;
                param.color = Color.black;
                handle.ExecuteAsync(param);
            }
            
            UnitFSMData unitFSMData = instigator.FSMBrain.GetAIData<UnitFSMData>();
            EAttackAttribute attackAttribute = unitFSMData.attackAttribute;
            unitFSMData.attackAttribute = EAttackAttribute.Lightning;

            for(int i = 0; i < targets.Count; i++)
            {
                UnitHealth health = targets[i];
                health.Attack(instigator, attackData);
                _ = new PlayHitFeedback(feedbackDataContainer, unitFSMData.attackAttribute, health.transform.position, Vector3.zero, unitFSMData.forwardDirection);
                await UniTask.Delay(TimeSpan.FromSeconds(.1f));
            }

            unitFSMData.attackAttribute = attackAttribute;
        }

        public void Despawn()
        {
            PoolManager.Despawn(poolReference);
            lineRenderer.positionCount = 0;
            outlineRenderer.positionCount = 0;
        }
    }
}
