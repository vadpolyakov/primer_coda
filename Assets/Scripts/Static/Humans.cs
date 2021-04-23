using GameHelpers;
using GameStaticValues;
using UnityEngine.Events;

namespace GameVariables
{
    public class Humans
    {
        public static UnityEvent onValueChange = new UnityEvent();

        private static long humans_value = DefaultValues.Humans;

        /// <summary>
        /// Общее колл-во
        /// </summary>
        public static long HumansValue
        {
            get { return humans_value; }
            set
            {
                humans_value = value;
                onValueChange.Invoke();
            }
        }

        public static string HappyString
        {
            get
            {
                return InString.TrueString(humans_value);
            }
        }
    }
}
