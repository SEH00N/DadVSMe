using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe
{
    [System.Serializable]
    public class UnitStat
    {
        [SerializeField]
        private EUnitStat statType;
        public EUnitStat StatType => statType;

        [SerializeField]
        private float defaultValue;
        public float DefaultValue
        {
            get => defaultValue;

            set
            {
                defaultValue = value;
                CalcFinalValue();
            }
        }
        [SerializeField]
        [HideInInspector]
        private float finalValue;
        public float FinalValue => finalValue;

        private Dictionary<float, int> addModifiers;
        private Dictionary<float, int> multiplyModifiers;

        [HideInInspector]
        public UnityEvent<float> onStatChanged;

        public UnitStat()
        {
            // this.defaultValue = 0;
            // this.finalValue = defaultValue;
            onStatChanged = null;
            addModifiers = new();
            multiplyModifiers = new();

            CalcFinalValue();
        }

        public UnitStat(float defaultValue)
        {
            this.defaultValue = defaultValue;
            this.finalValue = defaultValue;

            addModifiers = new();
            multiplyModifiers = new();
            onStatChanged = null;

            CalcFinalValue();
        }

        public void RegistAddModifier(float value)
        {
            if (addModifiers.ContainsKey(value))
                addModifiers[value]++;
            else
                addModifiers.Add(value, 1);

            CalcFinalValue();
        }

        public void RegistMultiplyModifier(float value)
        {
            if (multiplyModifiers.ContainsKey(value))
                multiplyModifiers[value]++;
            else
                multiplyModifiers.Add(value, 1);

            CalcFinalValue();
        }

        public void UnregistAddModifier(float value)
        {
            if (addModifiers.ContainsKey(value))
            {
                addModifiers[value]--;

                if (addModifiers[value] <= 0)
                    addModifiers.Remove(value);

                CalcFinalValue();
            }
        }

        public void UnregistMultiplyModifier(float value)
        {
            if (multiplyModifiers.ContainsKey(value))
            {
                multiplyModifiers[value]--;

                if (multiplyModifiers[value] <= 0)
                    multiplyModifiers.Remove(value);

                CalcFinalValue();
            }
        }

        public void CalcFinalValue()
        {
            float value = defaultValue;
            
            foreach (var addModifier in addModifiers)
            {
                value += addModifier.Key * addModifier.Value;
            }

            foreach (var mulModifier in multiplyModifiers)
            {
                value += defaultValue * mulModifier.Key * mulModifier.Value;
            }

            finalValue = value;

            onStatChanged?.Invoke(finalValue);
        }
    }
}