using UnityEngine;
using GameParametrs;
using GameSave;
using System.Collections;
using GameVariables;
using GameStaticValues;

namespace GameControllers
{
    public class MainController : MonoBehaviour
    {
        public static GameStatus CurrentStatus = GameStatus.None;

        public static BuildParametr AddBuildSelected = null;

        public void Start()
        {
            BuildController.LoadBuild();
            TileMapController.Start();

            Save.LoadGameState();
            Save.LoadBuildsAndTilemaps();

            RecalculateBonuses(true);

            StartCoroutine(AutoSaver());
        }

        private IEnumerator AutoSaver()
        {
            while(true)
            {
                yield return new WaitForSecondsRealtime(60);
                Save.SaveGameState();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Save.SaveGameState();
            }
        }

        public static void Click(Vector2 pos)
        {
            Vector3Int TilemapPos = TileMapController.WorldToTilemapPos(Camera.main.ScreenToWorldPoint(pos));

            switch(CurrentStatus)
            {
                case GameStatus.None:
                    ShowBuildInfo(TilemapPos);
                    break;
                case GameStatus.AddBuild:
                    AddBuild(TilemapPos);
                    break;
            }
        }

        public static void RecalculateBonuses(bool start)
        {
            double MoneyPerSec = DefaultValues.MoneyPerSec;
            double HappyValue = DefaultValues.Happy;
            long HumansValue = DefaultValues.Humans;

            foreach(var b in BuildController.AllBuilds)
            {
                MoneyPerSec += b.MoneyPerSecBonus * b.Count;
                HappyValue += b.HappyBonus * b.Count;
                HumansValue += b.HumansBonus * b.Count;
            }

            if (MoneyPerSec != Money.MoneyPerSec || start)
                Money.MoneyPerSec = MoneyPerSec;
            if (HappyValue != Happy.HappyValue || start)
                Happy.HappyValue = HappyValue;
            if (HumansValue != Humans.HumansValue || start)
                Humans.HumansValue = HumansValue;
        }

        private static void ShowBuildInfo(Vector3Int tilemap_pos)
        {
            BuildParametr build = TileMapController.GetBuild(tilemap_pos);
            if (build == null)
                return;

            Debug.Log(build.BuildTile + " [" + tilemap_pos + "]");
        }

        private static void AddBuild(Vector3Int tilemap_pos)
        {
            if (TileMapController.AddBuild(tilemap_pos, AddBuildSelected))
            {
                BuildController.BuyBuild(AddBuildSelected);
                RecalculateBonuses(false);
                Save.SaveGameState();
                Save.SaveBuildsAndTilemaps();
            }
            else
                return;
        }

        private static void UpdateBuild(Vector3Int tilemap_pos, BuildParametr new_build, BuildParametr old_build)
        {
            RecalculateBonuses(false);
            Save.SaveGameState();
            Save.SaveBuildsAndTilemaps();
        }

        private static void RemoveBuild(Vector3Int tilemap_pos)
        {
            RecalculateBonuses(false);
            Save.SaveGameState();
            Save.SaveBuildsAndTilemaps();
        }
    }

    public enum GameStatus
    {
        None,
        AddBuild
    }
}