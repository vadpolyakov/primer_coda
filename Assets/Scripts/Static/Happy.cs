using UnityEngine.Events;
using GameHelpers;
using GameStaticValues;

namespace GameVariables
{
    /// <summary>
    /// Уровень счастья
    /// </summary>
    public static class Happy
    {
        public static UnityEvent onValueChange = new UnityEvent();

        private static double happy_value = DefaultValues.Happy;
        private static double happy_per_sec_value = DefaultValues.HappyPerSec;

        /// <summary>
        /// Общее колл-во
        /// </summary>
        public static double HappyValue
        {
            get { return happy_value; }
            set
            {
                happy_value = value;
                happy_per_sec_value = value * Humans.HumansValue / 100;
                onValueChange.Invoke();
            }
        }

        /// <summary>
        /// Инкам в секунду
        /// </summary>
        public static double HappyPerSec
        {
            get { return happy_per_sec_value; }
        }

        public static string HappyString
        {
            get
            {
                return InString.TrueString(happy_value) + Symbols.HappySymbol;
            }
        }

        public static string HappyPerSecString
        {
            get
            {
                if (happy_per_sec_value >= 0)
                    return '+' + InString.TrueString(happy_per_sec_value) + Symbols.MoneySymbol + Symbols.PerSecSymbol;

                return InString.TrueString(happy_per_sec_value) + Symbols.MoneySymbol + Symbols.PerSecSymbol;
            }
        }
    }
}
