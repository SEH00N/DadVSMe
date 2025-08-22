using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace DadVSMe
{
    public static class EnumHelper
    {
        private static readonly Dictionary<Type, Array> enumValuesCacheByType = new Dictionary<Type, Array>();
        private static readonly Dictionary<Type, Dictionary<string, Enum>> enumValuesCacheByString = new Dictionary<Type, Dictionary<string, Enum>>();

        public static TEnum[] GetValues<TEnum>() where TEnum : Enum
        {
            Type type = typeof(TEnum);
            if(enumValuesCacheByType.TryGetValue(type, out Array enumValues) == false)
            {
                enumValues = Enum.GetValues(type);
                enumValuesCacheByType.Add(type, enumValues);
            }

            return (TEnum[])enumValues;
        }

        public static TEnum GetRandomValue<TEnum>() where TEnum : Enum
        {
            TEnum[] values = GetValues<TEnum>();
            return values[Random.Range(0, values.Length)];
        }

        public static TEnum Parse<TEnum>(string value) where TEnum : Enum
        {
            if(string.IsNullOrEmpty(value))
                return default;

            Type type = typeof(TEnum);
            if(enumValuesCacheByString.TryGetValue(type, out Dictionary<string, Enum> enumValues) == false)
            {
                enumValues = new Dictionary<string, Enum>();
                enumValuesCacheByString.Add(type, enumValues);
            }

            if(enumValues.TryGetValue(value, out Enum enumValue) == false)
            {
                enumValue = (Enum)Enum.Parse(typeof(TEnum), value);
                enumValues.Add(value, enumValue);
            }

            return (TEnum)enumValue;
        }
    }
}