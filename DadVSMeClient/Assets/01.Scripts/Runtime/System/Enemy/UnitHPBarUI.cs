using System.Collections.Generic;
using DadVSMe.Entities;
using DadVSMe.UI.HUD;
using H00N.AI.FSM;
using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe.UI
{
    public class UnitHPBarUI : MonoBehaviour
    {
        [SerializeField] float defaultOffset = 3.3f;

        [Space(10f)]
        [SerializeField] bool checkGrabState = false;
        [ConditionalField(nameof(checkGrabState), true, hideInInspector: true)]
        [SerializeField] List<FSMState> grabStates = null;
        [ConditionalField(nameof(checkGrabState), true, hideInInspector: true)]
        [SerializeField] float grabOffset = 1.5f;

        [Space(10f)]
        [SerializeField] Unit unit = null;
        [SerializeField] HPBarUI hpBarUI = null;

        private HashSet<FSMState> grabStatesSet = new HashSet<FSMState>();
        private float sign = 1f;

        private void Awake()
        {
            unit.OnInitializedEvent -= Initialize;
            unit.OnInitializedEvent += Initialize;
        }

        private void Initialize(IEntityData data)
        {
            hpBarUI.Initialize(unit);

            grabStatesSet.Clear();
            if(grabStates != null && grabStates.Count > 0)
            {
                foreach(FSMState state in grabStates)
                    grabStatesSet.Add(state);
            }
        }

        private void LateUpdate()
        {
            if(sign != Mathf.Sign(transform.lossyScale.x))
            {
                sign = Mathf.Sign(transform.lossyScale.x);

                Vector3 localScale = transform.localScale;
                localScale.x *= sign;
                transform.localScale = localScale;
            }

            if(unit == null || unit.FSMBrain == null)
                return;

            float offset = defaultOffset;
            if(checkGrabState)
            {
                if(grabStatesSet != null && grabStatesSet.Count > 0)
                {
                    if(grabStatesSet.Contains(unit.FSMBrain.CurrentState))
                        offset = grabOffset;
                }
            }

            Vector3 position = new Vector3(unit.transform.position.x, unit.transform.position.y + offset, unit.transform.position.z);
            transform.SetPositionAndRotation(position, Quaternion.identity);
        }
    }
}
