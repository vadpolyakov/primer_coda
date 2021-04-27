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

        public static bool LoadGameState()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName, GameStaticValues.Path.GameSaveName)))
                return false;
            GameState load = JsonUtility.FromJson<GameState>(File.ReadAllText(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName, GameStaticValues.Path.GameSaveName)));
            if (load.SaveVersion != GameStaticValues.DefaultValues.SaveVersion || double.IsNaN(load.Money))
                return false;

            GameVariables.Money.MoneyValue = load.Money;
            return true;
        }

        public static void SaveBuildsAndTilemaps()
        {
            TilemapsState save = new TilemapsState();

            var tilemap = GameControllers.TileMapController.GetTileMap(false);
            tilemap.ResizeBounds();
            for(int x = tilemap.cellBounds.xMin; x <= tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y <= tilemap.cellBounds.yMax; y++)
                {
                    int index = GameControllers.TileMapController.GetRoadIndex(new Vector3Int(x, 0, y));
                    if(index != 0)
                        save.RoadTiles.Add(new TileStateRoad(x, y, index));
                }
            }

            Dictionary<GameParametrs.BuildParametr, List<TileState>> builds_save = new Dictionary<GameParametrs.BuildParametr, List<TileState>>();
            foreach (var b in GameControllers.BuildController.AllBuilds)
            {
                builds_save.Add(b, new List<TileState>());
            }

            var build_tilemap = GameControllers.TileMapController.GetTileMap(true);
            build_tilemap.ResizeBounds();
            for(int x = build_tilemap.cellBounds.xMin; x <= build_tilemap.cellBounds.xMax; x++)
                for(int y = build_tilemap.cellBounds.yMin; y <= build_tilemap.cellBounds.yMax; y++)
                {
                    GameParametrs.BuildParametr build = GameControllers.TileMapController.GetBuild(new Vector3Int(x, 0, y));
                    if (build != null)
                        builds_save[build].Add(new TileState(x, y));
                }

            foreach(var b_s in builds_save)
            {
                if(b_s.Value.Count > 0)
                    save.BuildStates.Add(new BuildState(b_s.Key, b_s.Value));
            }

            check_path();
            File.WriteAllText(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName, GameStaticValues.Path.TIlemapsSaveName), JsonUtility.ToJson(save, GameStaticValues.DefaultValues.Debug));
        }

        public static bool LoadBuildsAndTilemaps()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName, GameStaticValues.Path.TIlemapsSaveName)))
                return false;

            TilemapsState load = JsonUtility.FromJson<TilemapsState>(File.ReadAllText(Path.Combine(Application.persistentDataPath, GameStaticValues.Path.SaveFolderName, GameStaticValues.Path.TIlemapsSaveName)));
            var road_tilemap = GameControllers.TileMapController.GetTileMap(false);
            foreach (var r in load.RoadTiles)
                GameControllers.TileMapController.SetRoadIndex(new Vector3Int(r.x_pos, 0, r.z_pos), r.ID);

            Dictionary<string, GameParametrs.BuildParametr> builds = new Dictionary<string, GameParametrs.BuildParametr>();
            foreach (var b in GameControllers.BuildController.AllBuilds)
                builds.Add(b.ID.ToString(), b);

            foreach(var b in load.BuildStates)
            {
                builds[b.ID].Count = b.Count;
                foreach(var t in b.Tiles)
                {
                    GameControllers.TileMapController.SetBuild(new Vector3Int(t.x_pos, 0, t.z_pos), builds[b.ID].BuildTile);
                }
            }
            return true;
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
    public class TilemapsState
    {
        public List<TileStateRoad> RoadTiles = new List<TileStateRoad>();
        public List<BuildState> BuildStates = new List<BuildState>();
    }
    [Serializable]
    public class TileState
    {
        public int x_pos;
        public int z_pos;
        public TileState(int x, int z)
        {
            x_pos = x;
            z_pos = z;
        }
    }
    [Serializable]
    public class TileStateRoad
    {
        public int x_pos;
        public int z_pos;
        public int ID;
        public TileStateRoad(int x, int z, int _id)
        {
            x_pos = x;
            z_pos = z;
            ID = _id;
        }
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
