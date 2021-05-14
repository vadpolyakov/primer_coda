using System.Collections.Generic;
using UnityEngine;
using GameStaticValues;

namespace GameControllers
{
    public class RandomCityGeneration : MonoBehaviour
    {
        private static List<Road> AllRoads;

        private static Road RandomRoad
        {
            get
            {
                return AllRoads[Random.Range(0, AllRoads.Count - 1)];
            }
        }

        public static void AddToRoad()
        {
            if (AllRoads == null)
                AllRoads = new List<Road>();
            if (AllRoads.Count == 0)
            {
                Debug.Log("!");
                AllRoads.Add(new Road(Vector3Int.zero));
                TileMapController.AddRoad(Vector3Int.zero);
                return;
            }

            Vector3Int roadpos = Vector3Int.zero;
            if (Road.Chance(MapGeneration.NewRoadChance))
            {
                Vector3Int firstPoint = Vector3Int.zero;
                while (true)
                {
                    Vector3Int direction = Vector3Int.zero;
                    firstPoint = RandomRoad.GetRandomAnglePoint(ref direction);
                    roadpos = firstPoint + direction;
                    if (TileMapController.can_build(roadpos))
                        break;
                }
                AllRoads.Add(new Road(firstPoint, roadpos));
                Debug.Log("+");
            }
            else
            {
                while(true)
                {
                    roadpos = RandomRoad.GetRandomPoint();
                    Debug.Log("++");
                    if (TileMapController.can_build(roadpos))
                        break;
                }
            }
            TileMapController.AddRoad(roadpos);
            Debug.Log(roadpos);
        }
    }

    public class Road
    {
        private bool firstIsEmpty = true;
        private List<Vector3Int> points;
        private int last_index { get { return points.Count - 1; } }

        public Road(Vector3Int point)
        {
            points = new List<Vector3Int> { point };
            firstIsEmpty = false;
        }

        public Road(Vector3Int startPoint, Vector3Int endPoint)
        {
            points = new List<Vector3Int>{ startPoint, endPoint };
            firstIsEmpty = true;
        }

        public Vector3Int GetRandomAnglePoint(ref Vector3Int direction)
        {
            if(last_index <= 2)
            {
                int index = Random.Range(0, 1);
                direction = GetRandomDirection(new List<Vector3Int> { GetNegativeDirection(points[0], points[1]) });
                return points[index];
            }
            else
            {
                int index = Random.Range(1, last_index - 1);
                direction = Get90DegDirection(GetPositiveDirection(points[index - 1], points[index]), GetPositiveDirection(points[index + 1], points[index]));
                return points[index];
            }
        }

        public Vector3Int GetRandomPoint()
        {
            Vector3Int direction = Vector3Int.zero;
            bool angle = true;

            if (Chance(50f))
            {
                direction = back_point(ref angle);
                if (angle)
                {
                    List<Vector3Int> buffer = new List<Vector3Int> { points[0] - direction };
                    buffer.AddRange(points);
                    points = buffer;
                }
                else
                    points[0] -= direction;

                return points[0];
            }
            else
            {
                direction = next_point(ref angle);
                if (angle)
                    points.Add(points[last_index] + direction);
                else
                    points[last_index] += direction;

                return points[last_index];
            }
        }

        private Vector3Int back_point(ref bool angle)
        {
            if (last_index == 0)
                return GetRandomDirection(new List<Vector3Int> { Vector3Int.right, Vector3Int.up });
            if (Vector3.Distance(points[0], points[1]) >= MapGeneration.RoadAngleMinDistance && Chance(MapGeneration.RoadAngleChance))
                return GetRandomDirection(new List<Vector3Int> { GetNegativeDirection(points[1], points[0]) });

            angle = false;
            return GetPositiveDirection(points[1], points[0]);
        }

        private Vector3Int next_point(ref bool angle)
        {
            if (last_index == 0)
                return GetRandomDirection(new List<Vector3Int> { Vector3Int.left, Vector3Int.down });
            if (Vector3.Distance(points[last_index - 1], points[last_index]) >= MapGeneration.RoadAngleMinDistance && Chance(MapGeneration.RoadAngleChance))
                return GetRandomDirection(new List<Vector3Int> { GetNegativeDirection(points[last_index - 1], points[last_index]) });

            angle = false;
            return GetPositiveDirection(points[last_index], points[last_index - 1]);
        }

        private static Vector3Int Get90DegDirection(Vector3Int fromDirection, Vector3Int toDirection)
        {
            if(fromDirection == Vector3Int.left || fromDirection == Vector3Int.right)
            {
                if(toDirection == Vector3Int.up)
                {
                    if (Chance(50f))
                        return Vector3Int.right;
                    else
                        return Vector3Int.down;
                }
                if(toDirection == Vector3Int.down)
                {
                    if (Chance(50f))
                        return Vector3Int.right;
                    else
                        return Vector3Int.up;
                }

                if (Chance(50f))
                    return Vector3Int.up;
                else
                    return Vector3Int.down;
            }

            else
            {
                if(toDirection == Vector3Int.left || toDirection == Vector3Int.right)
                {
                    if (Chance(50f))
                    {
                        if (toDirection == Vector3Int.left)
                            return Vector3Int.right;
                        else
                            return Vector3Int.left;
                    }
                    else
                    {
                        if (fromDirection == Vector3Int.up)
                            return Vector3Int.down;
                        else
                            return Vector3Int.up;
                    }
                }

                if (Chance(50f))
                    return Vector3Int.left;
                else
                    return Vector3Int.right;
            }
        }

        private static Vector3Int GetRandomDirection(List<Vector3Int> block)
        {
            List<Vector3Int> buffer = new List<Vector3Int>();
            if (!block.Contains(Vector3Int.left))
                buffer.Add(Vector3Int.left);
            if (!block.Contains(Vector3Int.right))
                buffer.Add(Vector3Int.right);
            if (!block.Contains(Vector3Int.up))
                buffer.Add(Vector3Int.up);
            if (!block.Contains(Vector3Int.down))
                buffer.Add(Vector3Int.down);
            return buffer[Random.Range(0, buffer.Count - 1)];
        }

        public static bool Chance(float chance)
        {
            float value = Random.Range(0f, 100f);
            if (value > chance)
                return false;
            else
                return true;
        }

        private static Vector3Int GetNegativeDirection(Vector3Int point1, Vector3Int point2)
        {
            return ConvertToVector3Int(-((Vector3)point1 - (Vector3)point2).normalized);
        }
        private static Vector3Int GetPositiveDirection(Vector3Int point1, Vector3Int point2)
        {
            return ConvertToVector3Int(((Vector3)point1 - (Vector3)point2).normalized);
        }
        private static Vector3Int ConvertToVector3Int(Vector3 v3)
        {
            return new Vector3Int((int)v3.x, (int)v3.y, (int)v3.z);
        }
    }
}