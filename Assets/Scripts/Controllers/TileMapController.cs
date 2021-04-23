using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameControllers
{
    public class TileMapController : MonoBehaviour
    {
        private enum MapTile
        {
            Grass,
            Water,
            Tree
        }
        private enum Reverse
        {
            No,
            Yes,
            Multi
        }

        private enum RoadType
        {
            None,
            Single,
            Vertical,
            Horizontal,
            LeftTop,
            LeftBot,
            RightTop,
            RightBot,
            TLeft,
            TRight,
            TTop,
            TBot,
            Four
        }

        private struct Tilemaps
        {
            public static Grid grid;

            public static Tilemap ground_map;

            public static Tilemap objects_map;
            public static Tilemap add_objects_map;

            public static Tilemap objects_map_reverse;
            public static Tilemap add_objects_map_reverse;

            public static Tilemap road_map;
            public static Tilemap add_road_map;

            public static void FindObjects()
            {
                grid = GameObject.Find("Grid").GetComponent<Grid>();

                ground_map = GameObject.Find("ground_map").GetComponent<Tilemap>();
                ground_map.ClearAllTiles();

                objects_map = GameObject.Find("objects_map").GetComponent<Tilemap>();
                objects_map.ClearAllTiles();

                add_objects_map = GameObject.Find("add_objects_map").GetComponent<Tilemap>();
                add_objects_map.ClearAllTiles();

                objects_map_reverse = GameObject.Find("objects_map_reverse").GetComponent<Tilemap>();
                objects_map_reverse.ClearAllTiles();

                add_objects_map_reverse = GameObject.Find("add_objects_map_reverse").GetComponent<Tilemap>();
                add_objects_map_reverse.ClearAllTiles();

                road_map = GameObject.Find("road_map").GetComponent<Tilemap>();
                road_map.ClearAllTiles();

                add_road_map = GameObject.Find("add_road_map").GetComponent<Tilemap>();
                add_road_map.ClearAllTiles();
            }
        }

        private struct Tiles
        {
            public static Tile grass_tile;
            public static Tile tree_tile;

            public struct Water
            {
                public static Tile Main;
                public static Tile LeftTop;
                public static Tile LeftBot;
                public static Tile RightTop;
                public static Tile RightBot;

                public static void Load()
                {
                    Main = Resources.Load<Tile>("Tiles/Water/water_main");
                    LeftTop = Resources.Load<Tile>("Tiles/Water/water_left_top");
                    LeftBot = Resources.Load<Tile>("Tiles/Water/water_left_bot");
                    RightTop = Resources.Load<Tile>("Tiles/Water/water_right_top");
                    RightBot = Resources.Load<Tile>("Tiles/Water/water_right_bot");
                }
            }

            public struct Road
            {
                public static Tile Single;
                public struct TwoSide
                {
                    public static Tile Horizontal;
                    public static Tile HorizontalLast;
                    public static Tile Vertical;
                    public static Tile VerticalLast;
                    public static Tile LeftTop;
                    public static Tile LeftBot;
                    public static Tile RightTop;
                    public static Tile RightBot;
                }
                public struct ThreeSide
                {
                    public static Tile Left;
                    public static Tile Right;
                    public static Tile Top;
                    public static Tile Bot;
                }
                public static Tile FourSide;

                public static void Load()
                {
                    Single = Resources.Load<Tile>("Tiles/Road/road_single");
                    TwoSide.Horizontal = Resources.Load<Tile>("Tiles/Road/road_horizontal");
                    TwoSide.HorizontalLast = Resources.Load<Tile>("Tiles/Road/road_horizontal_last");
                    TwoSide.Vertical = Resources.Load<Tile>("Tiles/Road/road_vertical");
                    TwoSide.VerticalLast = Resources.Load<Tile>("Tiles/Road/road_vertical_last");
                    TwoSide.LeftTop = Resources.Load<Tile>("Tiles/Road/road_left_top");
                    TwoSide.LeftBot = Resources.Load<Tile>("Tiles/Road/road_left_bot");
                    TwoSide.RightTop = Resources.Load<Tile>("Tiles/Road/road_right_top");
                    TwoSide.RightBot = Resources.Load<Tile>("Tiles/Road/road_right_bot");
                    ThreeSide.Left = Resources.Load<Tile>("Tiles/Road/road_t_left");
                    ThreeSide.Right = Resources.Load<Tile>("Tiles/Road/road_t_right");
                    ThreeSide.Top = Resources.Load<Tile>("Tiles/Road/road_t_top");
                    ThreeSide.Bot = Resources.Load<Tile>("Tiles/Road/road_t_bot");
                    FourSide = Resources.Load<Tile>("Tiles/Road/road_four_side");
                }

                public static RoadType GetType(Tile tile)
                {
                    if (tile == null) return RoadType.None;
                    if (tile == Single) return RoadType.Single;
                    if (tile == TwoSide.Horizontal) return RoadType.Horizontal;
                    if (tile == TwoSide.Vertical) return RoadType.Vertical;
                    if (tile == TwoSide.LeftTop) return RoadType.LeftTop;
                    if (tile == TwoSide.LeftBot) return RoadType.LeftBot;
                    if (tile == TwoSide.RightTop) return RoadType.RightTop;
                    if (tile == TwoSide.RightBot) return RoadType.RightBot;
                    if (tile == ThreeSide.Left) return RoadType.TLeft;
                    if (tile == ThreeSide.Right) return RoadType.TRight;
                    if (tile == ThreeSide.Top) return RoadType.TTop;
                    if (tile == ThreeSide.Bot) return RoadType.TBot;
                    return RoadType.Four;
                }

                public static RoadType GetType(Vector3Int position, bool correct = false)
                {
                    RoadType left = GetType(Tilemaps.add_road_map.GetTile<Tile>(position + Vector3Int.left));
                    if(left == RoadType.None)
                        left = GetType(Tilemaps.road_map.GetTile<Tile>(position + Vector3Int.left));
                    RoadType right = GetType(Tilemaps.add_road_map.GetTile<Tile>(position + Vector3Int.right));
                    if(right == RoadType.None)
                        right = GetType(Tilemaps.road_map.GetTile<Tile>(position + Vector3Int.right));
                    RoadType top = GetType(Tilemaps.add_road_map.GetTile<Tile>(position + Vector3Int.up));
                    if(top == RoadType.None)
                        top = GetType(Tilemaps.road_map.GetTile<Tile>(position + Vector3Int.up));
                    RoadType bot = GetType(Tilemaps.add_road_map.GetTile<Tile>(position + Vector3Int.down));
                    if(bot == RoadType.None)
                        bot = GetType(Tilemaps.road_map.GetTile<Tile>(position + Vector3Int.down));

                    int sideCount = 0;
                    if (left != RoadType.None)
                        sideCount++;
                    if (right != RoadType.None)
                        sideCount++;
                    if (top != RoadType.None)
                        sideCount++;
                    if (bot != RoadType.None)
                        sideCount++;

                    if(correct)
                    {
                        switch(left)
                        {
                            case RoadType.Single: SetRoadTile(position + Vector3Int.left, RoadType.Horizontal); break;
                            case RoadType.Vertical: SetRoadTile(position + Vector3Int.left, RoadType.TRight); break;
                            case RoadType.TLeft: SetRoadTile(position + Vector3Int.left, RoadType.Four); break;
                            case RoadType.LeftBot: SetRoadTile(position + Vector3Int.left, RoadType.TBot); break;
                            case RoadType.LeftTop: SetRoadTile(position + Vector3Int.left, RoadType.TTop); break;
                            default: break;
                        }
                        switch(right)
                        {
                            case RoadType.Single: SetRoadTile(position + Vector3Int.right, RoadType.Horizontal); break;
                            case RoadType.Vertical: SetRoadTile(position + Vector3Int.right, RoadType.TLeft); break;
                            case RoadType.TRight: SetRoadTile(position + Vector3Int.right, RoadType.Four); break;
                            case RoadType.RightBot: SetRoadTile(position + Vector3Int.right, RoadType.TBot); break;
                            case RoadType.RightTop: SetRoadTile(position + Vector3Int.right, RoadType.TTop); break;
                            default: break;
                        }
                        switch(top)
                        {
                            case RoadType.Single: SetRoadTile(position + Vector3Int.up, RoadType.Vertical); break;
                            case RoadType.Horizontal: SetRoadTile(position + Vector3Int.up, RoadType.TBot); break;
                            case RoadType.TTop: SetRoadTile(position + Vector3Int.up, RoadType.Four); break;
                            case RoadType.LeftTop: SetRoadTile(position + Vector3Int.up, RoadType.TLeft); break;
                            case RoadType.RightTop: SetRoadTile(position + Vector3Int.up, RoadType.TRight); break;
                            default: break;
                        }
                        switch (bot)
                        {
                            case RoadType.Single: SetRoadTile(position + Vector3Int.down, RoadType.Vertical); break;
                            case RoadType.Horizontal: SetRoadTile(position + Vector3Int.down, RoadType.TTop); break;
                            case RoadType.TBot: SetRoadTile(position + Vector3Int.down, RoadType.Four); break;
                            case RoadType.LeftBot: SetRoadTile(position + Vector3Int.down, RoadType.TLeft); break;
                            case RoadType.RightBot: SetRoadTile(position + Vector3Int.down, RoadType.TRight); break;
                            default: break;
                        }
                    }

                    switch(sideCount)
                    {
                        case 0: return RoadType.Single;
                        case 1: break;
                        case 2: goto two_side;
                        case 3: goto three_side;
                        case 4: return RoadType.Four;
                    }

                    if (left != RoadType.None || right != RoadType.None)
                        return RoadType.Horizontal;
                    else
                        return RoadType.Vertical;

                two_side:
                    if(left != RoadType.None)
                    {
                        if (right != RoadType.None)
                            return RoadType.Horizontal;
                        if (top != RoadType.None)
                            return RoadType.LeftTop;
                        return RoadType.LeftBot;
                    }
                    if(right != RoadType.None)
                    {
                        if (top != RoadType.None)
                            return RoadType.RightTop;
                        return RoadType.RightBot;
                    }
                    return RoadType.Vertical;
                three_side:
                    if (left == RoadType.None)
                        return RoadType.TRight;
                    if (right == RoadType.None)
                        return RoadType.TLeft;
                    if (bot == RoadType.None)
                        return RoadType.TTop;
                    return RoadType.TBot;
                }

                public static Tile GetTile(RoadType type)
                {
                    switch(type)
                    {
                        case RoadType.Single: return Single;
                        case RoadType.Vertical: return TwoSide.Vertical;
                        case RoadType.Horizontal: return TwoSide.Horizontal;
                        case RoadType.LeftTop: return TwoSide.LeftTop;
                        case RoadType.LeftBot: return TwoSide.LeftBot;
                        case RoadType.RightTop: return TwoSide.RightTop;
                        case RoadType.RightBot: return TwoSide.RightBot;
                        case RoadType.TLeft: return ThreeSide.Left;
                        case RoadType.TRight: return ThreeSide.Right;
                        case RoadType.TTop: return ThreeSide.Top;
                        case RoadType.TBot: return ThreeSide.Bot;
                        case RoadType.Four: return FourSide;
                        default: return null;
                    }
                }
            }

            public static void Load()
            {
                grass_tile = Resources.Load<Tile>("Tiles/Grass/Main");
                Water.Load();
                Road.Load();
            }
        }

        private struct History
        {
            public static List<Vector3Int> Road;
            public static List<Vector3Int> Builds;

        }

        private static bool can_build(Vector3Int pos)
        {
            return !Tilemaps.add_road_map.HasTile(pos) && !Tilemaps.road_map.HasTile(pos)
                && Tilemaps.ground_map.GetTile<Tile>(pos) == Tiles.grass_tile
                && !Tilemaps.objects_map.HasTile(pos) && !Tilemaps.objects_map_reverse.HasTile(-pos)
                && !Tilemaps.add_objects_map.HasTile(pos) && !Tilemaps.add_objects_map_reverse.HasTile(-pos);
        }

        private static void SetRoadTile(Vector3Int pos, RoadType type)
        {
            Tilemaps.add_road_map.SetTile(pos, Tiles.Road.GetTile(type));
        }
        private static void SetWaterTile(Vector3Int pos)
        {

        }

        public static void Start()
        {
            GameHelpers.InReady.TileMapControllerReady = false;

            Tilemaps.FindObjects();
            Tiles.Load();

            GameHelpers.InReady.TileMapControllerReady = true;
        }

        public static void GenerateMap()
        {
            for(int x = -100; x <= 100; x++)
                for(int z = -100; z <= 100; z++)
                {
                    float height = Mathf.PerlinNoise(x, z);
                    if(height >= .76f)
                    {
                        Tilemaps.ground_map.SetTile(new Vector3Int(x, 0, z), Tiles.tree_tile);
                        continue;
                    }
                    if(height <= .24)
                    {
                        SetWaterTile(new Vector3Int(x, 0, z));
                        continue;
                    }
                    Tilemaps.ground_map.SetTile(new Vector3Int(x, 0, z), Tiles.grass_tile);
                }
        }

        

        public static bool AddBuild(Vector3 mouseWorldPos, GameParametrs.BuildParametr build)
        {
            Vector3Int grid_pos = Tilemaps.grid.WorldToCell(mouseWorldPos);
            Vector3Int pos = new Vector3Int(grid_pos.x, grid_pos.y, 0);

            if (!can_build(pos))
                return false;

            Tilemaps.add_objects_map.SetTile(pos, build.BuildTile);

            return true;
        }

        public static void AddRoad(Vector3 mouseWorldPos)
        {
            Vector3Int grid_pos = Tilemaps.grid.WorldToCell(mouseWorldPos);
            Vector3Int pos = new Vector3Int(grid_pos.x, grid_pos.y, 0);
            SetRoadTile(pos, Tiles.Road.GetType(pos, true));
        }

        public static void ReverseBuild(Vector3 mouseWorldPos)
        {
            Vector3Int grid_pos = Tilemaps.grid.WorldToCell(mouseWorldPos);
            Vector3Int pos = new Vector3Int(grid_pos.x, grid_pos.y, 0);
        }
    }
}