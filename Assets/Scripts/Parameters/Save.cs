using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameSave
{
    public class Save
    {
        public static void SaveGameState()
        {
            GameState state = new GameState();
            check_path();
            File.WriteAllText(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName, GameStaticValues.Path.GameSaveName), JsonUtility.ToJson(new GameState(), GameStaticValues.DefaultValues.Debug));
        }

        private static void check_path()
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName)))
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName));
        }
    }

    [Serializable]
    public class GameState
    {
        public int SaveVersion;
        public double Money;
        public GameState()
        {
            Money = GameVariables.Money.MoneyValue;
            SaveVersion = GameStaticValues.DefaultValues.SaveVersion;
        }
    }
    [Serializable]
    public class RoadState
    {
        public List<TileStateRoad> Tiles = new List<TileStateRoad>();
    }
    [Serializable]
    public class TileState
    {
        public int x_pos;
        public int z_pos;
    }
    [Serializable]
    public class TileStateRoad
    {
        public int x_pos;
        public int z_pos;
        public int ID;
    }
    [Serializable]
    public class BuildState
    {
        public long Count;
        public string ID;
        public List<TileState> Tiles;
        public BuildState(GameParametrs.BuildParametr build, List<TileState> tiles)
        {
            Count = build.Count;
            ID = build.ID.ToString();
            Tiles = tiles;
        }

    }
}
