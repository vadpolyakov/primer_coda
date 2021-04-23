using UnityEngine.Events;
using GameHelpers;
using GameStaticValues;

namespace GameVariables
{
    /// <summary>
    /// Игровая валюта
    /// </summary>
    public static class Money
    {
        public static UnityEvent onValueChange = new UnityEvent();
        public static UnityEvent onPerSecValueChange = new UnityEvent();

        private static double money_value = DefaultValues.Money;
        private static double money_per_sec_value = DefaultValues.MoneyPerSec;

        /// <summary>
        /// Общее колл-во
        /// </summary>
        public static double MoneyValue
        {
            get { return money_value; }
            set
            {
                money_value = value;
                onValueChange.Invoke();
            }
        }

        /// <summary>
        /// Инкам в секунду
        /// </summary>
        public static double MoneyPerSec
        {
            get { return money_per_sec_value; }
            set
            {
                money_per_sec_value = value;
                onPerSecValueChange.Invoke();
            }
        }

        public static string MoneyString
        {
            get
            {
                return InString.TrueString(money_value) + Symbols.MoneySymbol;
            }
        }

        public static string MoneyPerSecString
        {
            get
            {
                if (money_per_sec_value >= 0)
                    return '+' + InString.TrueString(money_per_sec_value) + Symbols.MoneySymbol + Symbols.PerSecSymbol;

                return InString.TrueString(money_per_sec_value) + Symbols.MoneySymbol + Symbols.PerSecSymbol;
            }
        }
    }
}
