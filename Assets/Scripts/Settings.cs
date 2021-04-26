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
        public static int SaveVersion = 1;
        public static bool Debug = true;

        public static double Money = 10000;
        public static double MoneyPerSec = 0;

        public static double Happy = 100;
        public static double HappyPerSec = 0;

        public static long Humans = 0;
    }

    public struct MapGeneration
    {
        public static float MinGrassHeigh = .24f;
        public static float MaxGrassHeight = .76f;
    }

    public struct Path
    {
        public static string Builds = "Builds";
        public static string CityStates = "CityStates";
        public static string RoadTiles = "Tiles/Road";
        public static string WaterTiles = "Tiles/Water";

        public static string SaveFolderName = "save";
        public static string GameSaveName = "game_save.json";
    }
}