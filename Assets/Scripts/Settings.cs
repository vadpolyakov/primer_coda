public class Settings { }
namespace GameStaticValues
{
    public struct Symbols
    {
        public static char MoneySymbol = '$';
        public static char HappySymbol = '%';

        public static string PerSecSymbol = "/s";
    }

    public struct DefaultValues
    {
        public static double Money = 10000;
        public static double MoneyPerSec = 0;

        public static double Happy = 100;
        public static double HappyPerSec = 0;

        public static long Humans = 0;
    }
}