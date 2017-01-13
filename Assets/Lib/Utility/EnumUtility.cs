using System;
using Random = System.Random;

namespace UnityLib.Utility
{
    public static class EnumUtility
    {
        public static T GetRandomEnum<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            Random random = new Random();
            return (T)values.GetValue(random.Next(values.Length));
        }
    }
}