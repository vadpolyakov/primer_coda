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

        public static GameControllers.ShopController.ShopState ShopState = GameControllers.ShopController.ShopState.Builds;
        public static GameControllers.ShopController.ShopFilter ShopFilter = GameControllers.ShopController.ShopFilter.PerCost;
    }

    public struct MapGeneration
    {
        public static float MinGrassHeigh = .24f;
        public static float MaxGrassHeight = .76f;

        public static float NewRoadChance = 1f;
        public static float RoadAngleMinDistance = 5f;
        public static float RoadAngleChance = 35f;
    }

    public struct Path
    {
        public static string Builds = "Builds";
        public static string CityStates = "CityStates";
        public static string RoadTiles = "Tiles/Road";
        public static string WaterTiles = "Tiles/Water";

        public static string SaveFolderName = "save";
        public static string GameSaveName = "game_save.json";
        public static string TIlemapsSaveName = "tilemaps_save.json";
    }

    public struct CameraConfig
    {
        public static int FramesForClick = 5;
        public static float DistanceForDrag = 10f;
        public static float CamYSpeed = 10f;
        public static float ZoomMinBound = 7f;
        public static float ZoomMaxBound = 50f;
    }
}