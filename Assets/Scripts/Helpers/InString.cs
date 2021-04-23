namespace GameHelpers
{
    public static class InString
    {
        public static string TrueString(double value)
        {
            if (value < 10000)
                return value.ToString("0.#");
            if (value < 100000)
                return (value / 1000).ToString("0.##") + "k";
            if (value < 1000000)
                return (value / 1000).ToString("0.#") + "k";
            if (value < 100000000)
                return (value / 1000000).ToString("0.##") + "m";
            if (value < 1000000000)
                return (value / 1000000).ToString("0.#") + "m";
            if (value < 1000000000000)
                return (value / 1000000000).ToString("0.#") + "b";
            return (value / 1000000000000).ToString("0.#") + "kb";
        }

        public static string TrueString(long value)
        {
            if (value < 10000)
                return value.ToString("0.#");
            if (value < 100000)
                return (value / 1000).ToString("0.##") + "k";
            if (value < 1000000)
                return (value / 1000).ToString("0.#") + "k";
            if (value < 100000000)
                return (value / 1000000).ToString("0.##") + "m";
            if (value < 1000000000)
                return (value / 1000000).ToString("0.#") + "m";
            if (value < 1000000000000)
                return (value / 1000000000).ToString("0.#") + "b";
            return (value / 1000000000000).ToString("0.#") + "kb";
        }
    }
}
