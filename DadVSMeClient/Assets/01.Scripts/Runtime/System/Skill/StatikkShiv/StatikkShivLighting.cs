using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.Core.Cam;
using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class StatikkShivLighting : MonoBehaviour
    {
        private const int MAX_ATTACK_COUNT = 10;
        private const float LIFE_TIME_SECOND = 5f;

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

        public async void Active(Unit instigator, float attackRadius, IAttackData attackData, IAttackFeedbackDataContainer feedbackDataContainer)
        {
            if (instigator == null)
                return;

            List<Vector3> points = new();
            List<IHealth> targets = new();
            HashSet<Collider2D> checkedColliders = new();
            points.Add(instigator.transform.position);

            for (int i = 0; i < MAX_ATTACK_COUNT; i++)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(points[i], attackRadius);
                if (cols.Length == 0)
                    break;

                bool findTarget = false;
                foreach (var col in cols)
                {
                    if (targets.Count >= MAX_ATTACK_COUNT)
                        break;

                    if (col.gameObject == instigator.gameObject)
                        continue;

                    if (checkedColliders.Contains(col))
                        continue;

                    checkedColliders.Add(col);
                    if (col.TryGetComponent<IHealth>(out IHealth health))
                    {
                        if (targets.Contains(health))
                            continue;

                        points.Add(health.Position);
                        targets.Add(health);

                        findTarget = true;
                    }
                }

                if (targets.Count >= MAX_ATTACK_COUNT)
                    break;

                if (findTarget == false)
                    break;
            }

            AttackAsync(targets, instigator, attackData, feedbackDataContainer);
            lineRenderer.positionCount = targets.Count + 1;
            for (int i = 0; i <= targets.Count; i++)
                lineRenderer.SetPosition(i, points[i]);

            lineRendererAnimator.StartAnimationPerSegment(appearTime, disappearTime);

            await UniTask.Delay(TimeSpan.FromSeconds(LIFE_TIME_SECOND));

            Despawn();
        }

        private async void AttackAsync(List<IHealth> targets, Unit instigator, IAttackData attackData, IAttackFeedbackDataContainer feedbackDataContainer)
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
                IHealth health = targets[i];
                health.Attack(instigator, attackData);
                _ = new PlayHitFeedback(feedbackDataContainer, unitFSMData.attackAttribute, health.Position, Vector3.zero, unitFSMData.forwardDirection);
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
