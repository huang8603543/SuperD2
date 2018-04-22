using UnityEngine;
using GeoAPI.Geometries;
using System;
using NetTopologySuite.Noding;

namespace YunLang.SuperD.Core
{
    /// <summary>
    /// 墙计算类///
    /// </summary>
    public static class BasicWallCalc
    {
        /// <summary>
        /// 误差精度///
        /// </summary>
        public const float PRECISION = 0.001f;

        public static Coordinate Add(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X + b.X, a.Y + b.Y);
        }

        public static Coordinate Subtract(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X - b.X, a.Y - b.Y);
        }

        public static Coordinate Multi(Coordinate a, double b)
        {
            return new Coordinate(a.X * b, a.Y * b);
        }

        public static Coordinate Normalize(Coordinate a)
        {
            double num = Math.Sqrt((a.X * a.X) + (a.Y * a.Y));
            return new Coordinate(a.X / num, a.Y / num);
        }

        public static Vector2 Coordinate2Vector2(Coordinate coord)
        {
            return new Vector2((float)coord.X, (float)coord.Y);
        }

        public static Vector3 Coordinate2Vector3(Coordinate coord)
        {
            ///FloorPlanHelper.MakeDefaultPrecise(coord);
            return new Vector3((float)coord.X, (float)coord.Y, 0f);
        }

        public static Vector3 CreateFromCoordinate(Coordinate coord)
        {
            return new Vector3((float)coord.X, (float)coord.Y, 0f);
        }

        public static Coordinate Vector2Coord(Vector2 vect)
        {
            return new Coordinate(vect.x, vect.y);
        }

        public static Coordinate Vector2Coord(Vector3 vect)
        {
            return new Coordinate(vect.x, vect.y);
        }

        public static double DistanceCoord2Coord(Coordinate a, Coordinate b)
        {
            double xWeight = Math.Abs(a.X - b.X);
            double yWeight = Math.Abs(a.Y - b.Y);
            return Math.Sqrt((xWeight * xWeight) + (yWeight * yWeight));
        }

        public static double GetRealValue(double value, int resLen)
        {
            if (resLen == 0)
            {
                return (Math.Round(((value * 10.0) + 5.0)) / 10.0);
            }
            double num = Math.Pow(10.0, resLen);
            return (Math.Round(value * num) / num);
        }

        public static Coordinate GetRealValue(Coordinate coord, int resLen = 3)
        {
            return new Coordinate(GetRealValue(coord.X, resLen), GetRealValue(coord.Y, resLen));
        }

        /// <summary>
        /// 节点翻转（节点数为2时）///
        /// </summary>
        /// <param name="nss"></param>
        /// <returns></returns>
        public static NodedSegmentString Inverse(NodedSegmentString nss)
        {
            if (nss.Count > 2)
            {
                return nss;
            }
            return new NodedSegmentString(new Coordinate[] { nss.Coordinates[1], nss.Coordinates[0] }, null);
        }

        /// <summary>
        /// 两个墙段是否在一条直线上///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="pp1"></param>
        /// <param name="pp2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool IsOneLine(Coordinate p1, Coordinate p2, Coordinate pp1, Coordinate pp2, float precision = PRECISION)
        {
            if (Mathf.Abs((float)(((p1.X - pp1.X) * (p2.Y - pp1.Y)) - ((p2.X - pp1.X) * (p1.Y - pp1.Y)))) > precision)
            {
                return false;
            }
            if (Mathf.Abs((float)(((p1.X - pp2.X) * (p2.Y - pp2.Y)) - ((p2.X - pp2.X) * (p1.Y - pp2.Y)))) > precision)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 两个墙段是否平行///
        /// </summary>
        /// <param name="coord1"></param>
        /// <param name="coord2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool IsParallel(Coordinate coord1, Coordinate coord2, float precision = PRECISION)
        {
            return ((((coord1.X * coord2.Y) - (coord1.Y * coord2.X)) > -precision) && (((coord1.X * coord2.Y) - (coord1.Y * coord2.X)) < precision));
        }

        /// <summary>
        /// p点是否在墙段上///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool IsPointOnSegment(Coordinate p, Coordinate p1, Coordinate p2, float precision = PRECISION)
        {
            if ((((p.X - p1.X) > precision) && ((p.X - p2.X) > precision)) || (((p.X - p1.X) < -precision) && ((p.X - p2.X) < -precision)))
            {
                return false;
            }
            if ((((p.Y - p1.Y) > precision) && ((p.Y - p2.Y) > precision)) || (((p.Y - p1.Y) < -precision) && ((p.Y - p2.Y) < -precision)))
            {
                return false;
            }
            if (Mathf.Abs((float)(((p1.X - p.X) * (p2.Y - p.Y)) - ((p2.X - p.X) * (p1.Y - p.Y)))) > precision)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// p1和p2是否为同一点///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool IsSamePoint(Coordinate p1, Coordinate p2, float precision = PRECISION)
        {
            if ((p1 == null) || (p2 == null))
            {
                Debug.LogError("NULL POINT!");
                return false;
            }
            return ((Math.Abs(p1.X - p2.X) < precision) && (Math.Abs(p1.Y - p2.Y) < precision));
        }
    }
}
