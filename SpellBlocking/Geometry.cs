﻿
// From LeagueSharp.Common.Geometry
// C&P Link: https://github.com/LeagueSharp/LeagueSharp.Common

namespace SupportAIO.SpellBlocking
{
    #region

    using EnsoulSharp;
    using EnsoulSharp.SDK;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Color = System.Drawing.Color;

    #endregion

    /// <summary>
    ///     Provides methods regarding geometry math.
    /// </summary>
    public static class Geometry 
    {
        public static List<Vector2> To2DList(this Vector3[] v)
        {
            var result = new List<Vector2>();

            foreach (var point in v)
            {
                result.Add(point.ToVector2());
            }

            return result;
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Returns a Vector2 at center of the polygone.
        /// </summary>
        /// <param name="p">The polygon.</param>
        /// <returns></returns>
        public static Vector2 CenterOfPolygone(this Polygon p)
        {
            var cX = 0f;
            var cY = 0f;
            var pc = p.Points.Count;
            foreach (var point in p.Points)
            {
                cX += point.X;
                cY += point.Y;
            }
            return new Vector2(cX / pc, cY / pc);
        }

        /// <summary>
        ///     Returns the two intersection points between two circles.
        /// </summary>
        /// <param name="center1">The center1.</param>
        /// <param name="center2">The center2.</param>
        /// <param name="radius1">The radius1.</param>
        /// <param name="radius2">The radius2.</param>
        /// <returns></returns>
        public static Vector2[] CircleCircleIntersection(Vector2 center1, Vector2 center2, float radius1, float radius2)
        {
            var D = center1.Distance(center2);
            //The Circles dont intersect:
            if (D > radius1 + radius2 || (D <= Math.Abs(radius1 - radius2)))
            {
                return new Vector2[] { };
            }

            var A = (radius1 * radius1 - radius2 * radius2 + D * D) / (2 * D);
            var H = (float)Math.Sqrt(radius1 * radius1 - A * A);
            var Direction = (center2 - center1).Normalized();
            var PA = center1 + A * Direction;
            var S1 = PA + H * Direction.Perpendicular();
            var S2 = PA - H * Direction.Perpendicular();
            return new[] { S1, S2 };
        }

        /// <summary>
        ///     Clips the polygons.
        /// </summary>
        /// <param name="polygons">The polygons.</param>
        /// <returns></returns>
        public static List<List<IntPoint>> ClipPolygons(List<Polygon> polygons)
        {
            var subj = new List<List<IntPoint>>(polygons.Count);
            var clip = new List<List<IntPoint>>(polygons.Count);
            foreach (var polygon in polygons)
            {
                subj.Add(polygon.ToClipperPath());
                clip.Add(polygon.ToClipperPath());
            }
            var solution = new List<List<IntPoint>>();
            var c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);
            return solution;
        }

        /// <summary>
        ///     Checks if the two floats are close to each other.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="eps">The epsilon.</param>
        /// <returns></returns>
        public static bool Close(float a, float b, float eps)
        {
            if (Math.Abs(eps) < float.Epsilon)
            {
                eps = (float)1e-9;
            }
            return Math.Abs(a - b) <= eps;
        }

        /// <summary>
        ///     Returns the closest vector from a list.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="vList">The v list.</param>
        /// <returns></returns>
        public static Vector2 Closest(this Vector2 v, List<Vector2> vList)
        {
            var result = new Vector2();
            var dist = float.MaxValue;

            foreach (var vector in vList)
            {
                var distance = Vector2.DistanceSquared(v, vector);
                if (distance < dist)
                {
                    dist = distance;
                    result = vector;
                }
            }

            return result;
        }

        /// <summary>
        ///     Converts degrees to radians.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static float DegreeToRadian(double angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

        //AIBaseClient class extended methods:
        /// <summary>
        ///     Calculates the 2D distance to the unit.
        /// </summary>
        /// <param name="anotherUnit">Another unit.</param>
        /// <param name="squared">if set to <c>true</c> [squared].</param>
        /// <returns></returns>
        public static float Distance(AIBaseClient anotherUnit, bool squared = false)
        {
            var bestAllies = GameObjects.AllyHeroes
                .Where(t =>
                    t.Distance(ObjectManager.Player) < SupportAIO.Common.Champion.E.Range)
                .OrderBy(o => o.Health);
            
            return bestAllies.FirstOrDefault().Distance(anotherUnit, squared);
        }


        /// <summary>
            ///     Calculates the 2D distance to the unit.
            /// </summary>
            /// <param name="unit">The unit.</param>
            /// <param name="anotherUnit">Another unit.</param>
            /// <param name="squared">if set to <c>true</c> [squared].</param>
            /// <returns></returns>
            public static float Distance(this AIBaseClient unit, AIBaseClient anotherUnit, bool squared = false)
        {
            return unit.Position.ToVector2().Distance(anotherUnit.Position.ToVector2(), squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="anotherUnit">Another unit.</param>
        /// <param name="squared">if set to <c>true</c> [squared].</param>
        /// <returns></returns>
        public static float Distance(this AIBaseClient unit, AttackableUnit anotherUnit, bool squared = false)
        {
            return unit.Position.ToVector2().Distance(anotherUnit.Position.ToVector2(), squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the point.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="point">The point.</param>
        /// <param name="squared">if set to <c>true</c> [squared].</param>
        /// <returns></returns>
        public static float Distance(this AIBaseClient unit, Vector3 point, bool squared = false)
        {
            return unit.Position.ToVector2().Distance(point.ToVector2(), squared);
        }

        /// <summary>
        ///     Calculates the 2D distance to the point.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="point">The point.</param>
        /// <param name="squared">if set to <c>true</c> [squared].</param>
        /// <returns></returns>
        public static float Distance(this AIBaseClient unit, Vector2 point, bool squared = false)
        {
            return unit.Position.ToVector2().Distance(point, squared);
        }

        /// <summary>
        ///     Returns the 2D distance (XY plane) between two vector.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="other">The other.</param>
        /// <param name="squared">if set to <c>true</c> [squared].</param>
        /// <returns></returns>
        public static float Distance(this Vector3 v, Vector3 other, bool squared = false)
        {
            return v.ToVector2().Distance(other, squared);
        }

        /// <summary>
        ///     Calculates the distance to the Vector2.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="to">To.</param>
        /// <param name="squared">if set to <c>true</c> gets the distance squared.</param>
        /// <returns></returns>
        public static float Distance(this Vector2 v, Vector2 to, bool squared = false)
        {
            return squared ? Vector2.DistanceSquared(v, to) : Vector2.Distance(v, to);
        }

        /// <summary>
        ///     Calculates the distance to the Vector3.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="to">To.</param>
        /// <param name="squared">if set to <c>true</c> gets the distance squared.</param>
        /// <returns></returns>
        public static float Distance(this Vector2 v, Vector3 to, bool squared = false)
        {
            return v.Distance(to.ToVector2(), squared);
        }

        /// <summary>
        ///     Calculates the distance to the unit.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="to">To.</param>
        /// <param name="squared">if set to <c>true</c> gets the distance squared.</param>
        /// <returns></returns>
        public static float Distance(this Vector2 v, AIBaseClient to, bool squared = false)
        {
            return v.Distance(to.Position.ToVector2(), squared);
        }

        /// <summary>
        ///     Returns the distance to the line segment.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="segmentStart">The segment start.</param>
        /// <param name="segmentEnd">The segment end.</param>
        /// <param name="onlyIfOnSegment">if set to <c>true</c> [only if on segment].</param>
        /// <param name="squared">if set to <c>true</c> [squared].</param>
        /// <returns></returns>
        public static float Distance(
            this Vector2 point,
            Vector2 segmentStart,
            Vector2 segmentEnd,
            bool onlyIfOnSegment = false,
            bool squared = false)
        {
            var objects = point.ProjectOn(segmentStart, segmentEnd);

            if (objects.IsOnSegment || onlyIfOnSegment == false)
            {
                return squared
                           ? Vector2.DistanceSquared(objects.SegmentPoint, point)
                           : Vector2.Distance(objects.SegmentPoint, point);
            }
            return float.MaxValue;
        }

        /// <summary>
        ///     Calculates the 3D distance to the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="anotherUnit">Another unit.</param>
        /// <param name="squared">if set to <c>true</c> [squared].</param>
        /// <returns></returns>
        public static float Distance3D(this AIBaseClient unit, AIBaseClient anotherUnit, bool squared = false)
        {
            return squared
                       ? Vector3.DistanceSquared(unit.Position, anotherUnit.Position)
                       : Vector3.Distance(unit.Position, anotherUnit.Position);
        }

        //Vector2 class extended methods:

        /// <summary>
        ///     Returns true if the vector is valid.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns></returns>
        public static bool IsValid(this Vector2 v)
        {
            return v != Vector2.Zero;
        }

        /// <summary>
        ///     Determines whether this instance is valid.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns></returns>
        public static bool IsValid(this Vector3 v)
        {
            return v != Vector3.Zero;
        }

        /// <summary>
        ///     Joins all the polygones in the list in one polygone if they interect.
        /// </summary>
        /// <param name="sList">The polygon list.</param>
        /// <returns></returns>
        public static List<Polygon> JoinPolygons(this List<Polygon> sList)
        {
            var p = ClipPolygons(sList);
            var tList = new List<List<IntPoint>>();

            var c = new Clipper();
            c.AddPaths(p, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, tList, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            return ToPolygons(tList);
        }

        /// <summary>
        ///     Joins all the polygones.
        ///     ClipType: http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/ClipType.htm
        ///     PolyFillType: http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/PolyFillType.htm
        /// </summary>
        /// <param name="sList">The s list.</param>
        /// <param name="cType">Type of the c.</param>
        /// <param name="pType">Type of the p.</param>
        /// <param name="pFType1">The p f type1.</param>
        /// <param name="pFType2">The p f type2.</param>
        /// <returns></returns>
        public static List<Polygon> JoinPolygons(
            this List<Polygon> sList,
            ClipType cType,
            PolyType pType = PolyType.ptClip,
            PolyFillType pFType1 = PolyFillType.pftNonZero,
            PolyFillType pFType2 = PolyFillType.pftNonZero)
        {
            var p = ClipPolygons(sList);
            var tList = new List<List<IntPoint>>();

            var c = new Clipper();
            c.AddPaths(p, pType, true);
            c.Execute(cType, tList, pFType1, pFType2);

            return ToPolygons(tList);
        }

        /// <summary>
        ///     Moves the polygone to the set position. It dosent rotate the polygone.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="moveTo">The move to.</param>
        /// <returns></returns>
        public static Polygon MovePolygone(this Polygon polygon, Vector2 moveTo)
        {
            var p = new Polygon();

            p.Add(moveTo);

            var count = polygon.Points.Count;

            var startPoint = polygon.Points[0];

            for (var i = 1; i < count; i++)
            {
                var polygonePoint = polygon.Points[i];

                p.Add(
                    new Vector2(
                        moveTo.X + (polygonePoint.X - startPoint.X),
                        moveTo.Y + (polygonePoint.Y - startPoint.Y)));
            }
            return p;
        }

        /// <summary>
        ///     Normalizes the specified vector.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns></returns>
        public static Vector3 Normalized(this Vector3 v)
        {
            v.Normalize();
            return v;
        }

        /// <summary>
        ///     Returns the total distance of a path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static float PathLength(this List<Vector2> path)
        {
            var distance = 0f;
            for (var i = 0; i < path.Count - 1; i++)
            {
                distance += path[i].Distance(path[i + 1]);
            }
            return distance;
        }

        /// <summary>
        ///     Returns the perpendicular vector.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        /// <summary>
        ///     Returns the second perpendicular vector.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns></returns>
        public static Vector2 Perpendicular2(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        /// <summary>
        ///     Returns the polar for vector angle (in Degrees).
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <returns></returns>
        public static float Polar(this Vector2 v1)
        {
            if (Close(v1.X, 0, 0))
            {
                if (v1.Y > 0)
                {
                    return 90;
                }
                return v1.Y < 0 ? 270 : 0;
            }

            var theta = RadianToDegree(Math.Atan((v1.Y) / v1.X));
            if (v1.X < 0)
            {
                theta = theta + 180;
            }
            if (theta < 0)
            {
                theta = theta + 360;
            }
            return theta;
        }

        /// <summary>
        ///     Returns the position where the vector will be after t(time) with s(speed) and delay.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="t">The time.</param>
        /// <param name="s">The speed.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        public static Vector2 PositionAfter(this List<Vector2> self, int t, int s, int delay = 0)
        {
            var distance = Math.Max(0, t - delay) * s / 1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var from = self[i];
                var to = self[i + 1];
                var d = (int)to.Distance(from);
                if (d > distance)
                {
                    return from + distance * (to - from).Normalized();
                }
                distance -= d;
            }
            return self[self.Count - 1];
        }

        /// <summary>
        ///     Converts radians to degrees.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static float RadianToDegree(double angle)
        {
            return (float)(angle * (180.0 / Math.PI));
        }

        /// <summary>
        ///     Rotates the vector around the set position.
        ///     Angle is in radians.
        /// </summary>
        /// <param name="rotated">The rotated.</param>
        /// <param name="around">The around.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static Vector2 RotateAroundPoint(this Vector2 rotated, Vector2 around, float angle)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);

            var x = cos * (rotated.X - around.X) - sin * (rotated.Y - around.Y) + around.X;
            var y = sin * (rotated.X - around.X) + cos * (rotated.Y - around.Y) + around.Y;

            return new Vector2((float)x, (float)y);
        }

        /// <summary>
        ///     Rotates the polygon around the set position.
        ///     Angle is in radians.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="around">The around.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, float angle)
        {
            var p = new Polygon();

            foreach (var polygonePoint in polygon.Points.Select(poinit => RotateAroundPoint(poinit, around, angle)))
            {
                p.Add(polygonePoint);
            }
            return p;
        }

        /// <summary>
        ///     Rotates the polygon around to the set direction.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="around">The around.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, Vector2 direction)
        {
            var deltaX = around.X - direction.X;
            var deltaY = around.Y - direction.Y;
            var angle = (float)Math.Atan2(deltaY, deltaX);
            return RotatePolygon(polygon, around, angle - DegreeToRadian(90));
        }

        /// <summary>
        ///     Sets the z.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Vector3 SetZ(this Vector3 v, float? value = null)
        {
            if (value == null)
            {
                v.Z = Game.CursorPosCenter.Z;
            }
            else
            {
                v.Z = (float)value;
            }
            return v;
        }

        /// <summary>
        ///     Shortens the specified vector.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="to">The vector to shorten from.</param>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static Vector2 Shorten(this Vector2 v, Vector2 to, float distance)
        {
            return v - distance * (to - v).Normalized();
        }

        /// <summary>
        ///     Shortens the specified vector.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="to">The vector to shorten from.</param>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static Vector3 Shorten(this Vector3 v, Vector3 to, float distance)
        {
            return v - distance * (to - v).Normalized();
        }

        /// <summary>
        ///     Switches the Y and Z.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns></returns>
        public static Vector3 SwitchYZ(this Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        //Vector3 class extended methods:
        /// <summary>
        ///     Converts a 3D path to 2D
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static List<Vector2> To2D(this List<Vector3> path)
        {
            return path.Select(point => point.ToVector2()).ToList();
        }
        /// <summary>
        ///     Converts the Vector2 to Vector3. (Z = NavMesh.GetHeightForPosition)
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns></returns>
        public static Vector3 To3D2(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, NavMesh.GetHeightForPosition(v.X, v.Y));
        }

        /// <summary>
        ///     Converts a list of <see cref="IntPoint" /> to a polygon.
        /// </summary>
        /// <param name="v">The int points.</param>
        /// <returns></returns>
        public static Polygon ToPolygon(this List<IntPoint> v)
        {
            var polygon = new Polygon();
            foreach (var point in v)
            {
                polygon.Add(new Vector2(point.X, point.Y));
            }
            return polygon;
        }

        /// <summary>
        ///     Converts a list of list points to a polygon.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static List<Polygon> ToPolygons(this List<List<IntPoint>> v)
        {
            return v.Select(path => path.ToPolygon()).ToList();
        }

        /// <summary>
        ///     Gets the vectors movement collision.
        /// </summary>
        /// <param name="startPoint1">The start point1.</param>
        /// <param name="endPoint1">The end point1.</param>
        /// <param name="v1">The v1.</param>
        /// <param name="startPoint2">The start point2.</param>
        /// <param name="v2">The v2.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        public static Object[] VectorMovementCollision(
            Vector2 startPoint1,
            Vector2 endPoint1,
            float v1,
            Vector2 startPoint2,
            float v2,
            float delay = 0f)
        {
            float sP1x = startPoint1.X,
                  sP1y = startPoint1.Y,
                  eP1x = endPoint1.X,
                  eP1y = endPoint1.Y,
                  sP2x = startPoint2.X,
                  sP2y = startPoint2.Y;

            float d = eP1x - sP1x, e = eP1y - sP1y;
            float dist = (float)Math.Sqrt(d * d + e * e), t1 = float.NaN;
            float S = Math.Abs(dist) > float.Epsilon ? v1 * d / dist : 0,
                  K = (Math.Abs(dist) > float.Epsilon) ? v1 * e / dist : 0f;

            float r = sP2x - sP1x, j = sP2y - sP1y;
            var c = r * r + j * j;

            if (dist > 0f)
            {
                if (Math.Abs(v1 - float.MaxValue) < float.Epsilon)
                {
                    var t = dist / v1;
                    t1 = v2 * t >= 0f ? t : float.NaN;
                }
                else if (Math.Abs(v2 - float.MaxValue) < float.Epsilon)
                {
                    t1 = 0f;
                }
                else
                {
                    float a = S * S + K * K - v2 * v2, b = -r * S - j * K;

                    if (Math.Abs(a) < float.Epsilon)
                    {
                        if (Math.Abs(b) < float.Epsilon)
                        {
                            t1 = (Math.Abs(c) < float.Epsilon) ? 0f : float.NaN;
                        }
                        else
                        {
                            var t = -c / (2 * b);
                            t1 = (v2 * t >= 0f) ? t : float.NaN;
                        }
                    }
                    else
                    {
                        var sqr = b * b - a * c;
                        if (sqr >= 0)
                        {
                            var nom = (float)Math.Sqrt(sqr);
                            var t = (-nom - b) / a;
                            t1 = v2 * t >= 0f ? t : float.NaN;
                            t = (nom - b) / a;
                            var t2 = (v2 * t >= 0f) ? t : float.NaN;

                            if (!float.IsNaN(t2) && !float.IsNaN(t1))
                            {
                                if (t1 >= delay && t2 >= delay)
                                {
                                    t1 = Math.Min(t1, t2);
                                }
                                else if (t2 >= delay)
                                {
                                    t1 = t2;
                                }
                            }
                        }
                    }
                }
            }
            else if (Math.Abs(dist) < float.Epsilon)
            {
                t1 = 0f;
            }

            return new Object[] { t1, (!float.IsNaN(t1)) ? new Vector2(sP1x + S * t1, sP1y + K * t1) : new Vector2() };
        }

        #endregion

        /// <summary>
        ///     Represents an intersection result.
        /// </summary>
        public struct IntersectionResult
        {
            #region Fields

            /// <summary>
            ///     If they intersect.
            /// </summary>
            public bool Intersects;

            /// <summary>
            ///     The point
            /// </summary>
            public Vector2 Point;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="IntersectionResult" /> struct.
            /// </summary>
            /// <param name="Intersects">if set to <c>true</c>, they insersect.</param>
            /// <param name="Point">The point.</param>
            public IntersectionResult(bool Intersects = false, Vector2 Point = new Vector2())
            {
                this.Intersects = Intersects;
                this.Point = Point;
            }

            #endregion
        }

        /// <summary>
        ///     Represents the projection information.
        /// </summary>
        public struct ProjectionInfo
        {
            #region Fields

            /// <summary>
            ///     The is on segment
            /// </summary>
            public bool IsOnSegment;

            /// <summary>
            ///     The line point
            /// </summary>
            public Vector2 LinePoint;

            /// <summary>
            ///     The segment point
            /// </summary>
            public Vector2 SegmentPoint;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="ProjectionInfo" /> struct.
            /// </summary>
            /// <param name="isOnSegment">if set to <c>true</c> [is on segment].</param>
            /// <param name="segmentPoint">The segment point.</param>
            /// <param name="linePoint">The line point.</param>
            public ProjectionInfo(bool isOnSegment, Vector2 segmentPoint, Vector2 linePoint)
            {
                this.IsOnSegment = isOnSegment;
                this.SegmentPoint = segmentPoint;
                this.LinePoint = linePoint;
            }

            #endregion
        }

        /// <summary>
        ///     Represents a polygon.
        /// </summary>
        public class Polygon
        {
            #region Fields

            /// <summary>
            ///     The points
            /// </summary>
            public List<Vector2> Points = new List<Vector2>();

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Adds the specified point.
            /// </summary>
            /// <param name="point">The point.</param>
            public void Add(Vector2 point)
            {
                this.Points.Add(point);
            }

            /// <summary>
            ///     Adds the specified point.
            /// </summary>
            /// <param name="point">The point.</param>
            public void Add(Vector3 point)
            {
                this.Points.Add(point.ToVector2());
            }

            /// <summary>
            ///     Adds the specified polygon.
            /// </summary>
            /// <param name="polygon">The polygon.</param>
            public void Add(Polygon polygon)
            {
                foreach (var point in polygon.Points)
                {
                    this.Points.Add(point);
                }
            }

            /// <summary>
            ///     Draws the polygon.
            /// </summary>
            /// <param name="color">The color.</param>
            /// <param name="width">The width.</param>
            public virtual void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= this.Points.Count - 1; i++)
                {
                    var nextIndex = (this.Points.Count - 1 == i) ? 0 : (i + 1);

                }
            }

            /// <summary>
            ///     Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(Vector2 point)
            {
                return !this.IsOutside(point);
            }

            /// <summary>
            ///     Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(Vector3 point)
            {
                return !this.IsOutside(point.ToVector2());
            }

            /// <summary>
            ///     Determines whether the specified point is inside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsInside(GameObject point)
            {
                return !this.IsOutside(point.Position.ToVector2());
            }

            /// <summary>
            ///     Determines whether the specified point is outside.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public bool IsOutside(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, this.ToClipperPath()) != 1;
            }

            /// <summary>
            ///     Converts this instance to a clipper path.
            /// </summary>
            /// <returns></returns>
            public List<IntPoint> ToClipperPath()
            {
                var result = new List<IntPoint>(this.Points.Count);
                result.AddRange(this.Points.Select(point => new IntPoint(point.X, point.Y)));
                return result;
            }

            #endregion

            /// <summary>
            ///     Represnets an arc polygon.
            /// </summary>
            public class Arc : Polygon
            {
                #region Fields

                /// <summary>
                ///     The angle
                /// </summary>
                public float Angle;

                /// <summary>
                ///     The end position
                /// </summary>
                public Vector2 EndPos;

                /// <summary>
                ///     The radius
                /// </summary>
                public float Radius;

                /// <summary>
                ///     The start position
                /// </summary>
                public Vector2 StartPos;

                /// <summary>
                ///     The quality
                /// </summary>
                private readonly int _quality;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Arc" /> class.
                /// </summary>
                /// <param name="start">The start.</param>
                /// <param name="direction">The direction.</param>
                /// <param name="angle">The angle.</param>
                /// <param name="radius">The radius.</param>
                /// <param name="quality">The quality.</param>
                public Arc(Vector3 start, Vector3 direction, float angle, float radius, int quality = 20)
                    : this(start.ToVector2(), direction.ToVector2(), angle, radius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Arc" /> class.
                /// </summary>
                /// <param name="start">The start.</param>
                /// <param name="direction">The direction.</param>
                /// <param name="angle">The angle.</param>
                /// <param name="radius">The radius.</param>
                /// <param name="quality">The quality.</param>
                public Arc(Vector2 start, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    this.StartPos = start;
                    this.EndPos = (direction - start).Normalized();
                    this.Angle = angle;
                    this.Radius = radius;
                    this._quality = quality;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">The offset.</param>
                public void UpdatePolygon(int offset = 0)
                {
                    this.Points.Clear();
                    var outRadius = (this.Radius + offset) / (float)Math.Cos(2 * Math.PI / this._quality);
                    var side1 = this.EndPos.Rotated(-this.Angle * 0.5f);
                    for (var i = 0; i <= this._quality; i++)
                    {
                        var cDirection = side1.Rotated(i * this.Angle / this._quality).Normalized();
                        this.Points.Add(
                            new Vector2(
                                this.StartPos.X + outRadius * cDirection.X,
                                this.StartPos.Y + outRadius * cDirection.Y));
                    }
                }

                #endregion
            }

            /// <summary>
            ///     Represents a circle polygon.
            /// </summary>
            public class Circle : Polygon
            {
                #region Fields

                /// <summary>
                ///     The center
                /// </summary>
                public Vector2 Center;

                /// <summary>
                ///     The radius
                /// </summary>
                public float Radius;

                /// <summary>
                ///     The quality
                /// </summary>
                private readonly int _quality;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Circle" /> class.
                /// </summary>
                /// <param name="center">The center.</param>
                /// <param name="radius">The radius.</param>
                /// <param name="quality">The quality.</param>
                public Circle(Vector3 center, float radius, int quality = 20)
                    : this(center.ToVector2(), radius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Circle" /> class.
                /// </summary>
                /// <param name="center">The center.</param>
                /// <param name="radius">The radius.</param>
                /// <param name="quality">The quality.</param>
                public Circle(Vector2 center, float radius, int quality = 20)
                {
                    this.Center = center;
                    this.Radius = radius;
                    this._quality = quality;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">The offset.</param>
                /// <param name="overrideWidth">Width of the override.</param>
                public void UpdatePolygon(int offset = 0, float overrideWidth = -1)
                {
                    this.Points.Clear();
                    var outRadius = (overrideWidth > 0
                                         ? overrideWidth
                                         : (offset + this.Radius) / (float)Math.Cos(2 * Math.PI / this._quality));
                    for (var i = 1; i <= this._quality; i++)
                    {
                        var angle = i * 2 * Math.PI / this._quality;
                        var point = new Vector2(
                            this.Center.X + outRadius * (float)Math.Cos(angle),
                            this.Center.Y + outRadius * (float)Math.Sin(angle));
                        this.Points.Add(point);
                    }
                }

                #endregion
            }

            /// <summary>
            ///     Represents a line polygon.
            /// </summary>
            public class Line : Polygon
            {
                #region Fields

                /// <summary>
                ///     The line end
                /// </summary>
                public Vector2 LineEnd;

                /// <summary>
                ///     The line start
                /// </summary>
                public Vector2 LineStart;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Line" /> class.
                /// </summary>
                /// <param name="start">The start.</param>
                /// <param name="end">The end.</param>
                /// <param name="length">The length.</param>
                public Line(Vector3 start, Vector3 end, float length = -1)
                    : this(start.ToVector2(), end.ToVector2(), length)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Line" /> class.
                /// </summary>
                /// <param name="start">The start.</param>
                /// <param name="end">The end.</param>
                /// <param name="length">The length.</param>
                public Line(Vector2 start, Vector2 end, float length = -1)
                {
                    this.LineStart = start;
                    this.LineEnd = end;
                    if (length > 0)
                    {
                        this.Length = length;
                    }
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets or sets the length.
                /// </summary>
                /// <value>
                ///     The length.
                /// </value>
                public float Length
                {
                    get
                    {
                        return this.LineStart.Distance(this.LineEnd);
                    }
                    set
                    {
                        this.LineEnd = (this.LineEnd - this.LineStart).Normalized() * value + this.LineStart;
                    }
                }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                public void UpdatePolygon()
                {
                    this.Points.Clear();
                    this.Points.Add(this.LineStart);
                    this.Points.Add(this.LineEnd);
                }

                #endregion
            }

            /// <summary>
            ///     Represents a rectangle polygon.
            /// </summary>
            public class Rectangle : Polygon
            {
                #region Fields

                /// <summary>
                ///     The end
                /// </summary>
                public Vector2 End;

                /// <summary>
                ///     The start
                /// </summary>
                public Vector2 Start;

                /// <summary>
                ///     The width
                /// </summary>
                public float Width;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Rectangle" /> class.
                /// </summary>
                /// <param name="start">The start.</param>
                /// <param name="end">The end.</param>
                /// <param name="width">The width.</param>
                public Rectangle(Vector3 start, Vector3 end, float width)
                    : this(start.ToVector2(), end.ToVector2(), width)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Rectangle" /> class.
                /// </summary>
                /// <param name="start">The start.</param>
                /// <param name="end">The end.</param>
                /// <param name="width">The width.</param>
                public Rectangle(Vector2 start, Vector2 end, float width)
                {
                    this.Start = start;
                    this.End = end;
                    this.Width = width;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Properties

                /// <summary>
                ///     Gets the direction.
                /// </summary>
                /// <value>
                ///     The direction.
                /// </value>
                public Vector2 Direction
                {
                    get
                    {
                        return (this.End - this.Start).Normalized();
                    }
                }

                /// <summary>
                ///     Gets the perpendicular.
                /// </summary>
                /// <value>
                ///     The perpendicular.
                /// </value>
                public Vector2 Perpendicular
                {
                    get
                    {
                        return this.Direction.Perpendicular();
                    }
                }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">The offset.</param>
                /// <param name="overrideWidth">Width of the override.</param>
                public void UpdatePolygon(int offset = 0, float overrideWidth = -1)
                {
                    this.Points.Clear();
                    this.Points.Add(
                        this.Start + (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                        - offset * this.Direction);
                    this.Points.Add(
                        this.Start - (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                        - offset * this.Direction);
                    this.Points.Add(
                        this.End - (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                        + offset * this.Direction);
                    this.Points.Add(
                        this.End + (overrideWidth > 0 ? overrideWidth : this.Width + offset) * this.Perpendicular
                        + offset * this.Direction);
                }

                #endregion
            }

            /// <summary>
            ///     Represents a ring polygon.
            /// </summary>
            public class Ring : Polygon
            {
                #region Fields

                /// <summary>
                ///     The center
                /// </summary>
                public Vector2 Center;

                /// <summary>
                ///     The inner radius
                /// </summary>
                public float InnerRadius;

                /// <summary>
                ///     The outer radius
                /// </summary>
                public float OuterRadius;

                /// <summary>
                ///     The quality
                /// </summary>
                private readonly int _quality;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Ring" /> class.
                /// </summary>
                /// <param name="center">The center.</param>
                /// <param name="innerRadius">The inner radius.</param>
                /// <param name="outerRadius">The outer radius.</param>
                /// <param name="quality">The quality.</param>
                public Ring(Vector3 center, float innerRadius, float outerRadius, int quality = 20)
                    : this(center.ToVector2(), innerRadius, outerRadius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Ring" /> class.
                /// </summary>
                /// <param name="center">The center.</param>
                /// <param name="innerRadius">The inner radius.</param>
                /// <param name="outerRadius">The outer radius.</param>
                /// <param name="quality">The quality.</param>
                public Ring(Vector2 center, float innerRadius, float outerRadius, int quality = 20)
                {
                    this.Center = center;
                    this.InnerRadius = innerRadius;
                    this.OuterRadius = outerRadius;
                    this._quality = quality;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">The offset.</param>
                public void UpdatePolygon(int offset = 0)
                {
                    this.Points.Clear();
                    var outRadius = (offset + this.InnerRadius + this.OuterRadius)
                                    / (float)Math.Cos(2 * Math.PI / this._quality);
                    var innerRadius = this.InnerRadius - this.OuterRadius - offset;
                    for (var i = 0; i <= this._quality; i++)
                    {
                        var angle = i * 2 * Math.PI / this._quality;
                        var point = new Vector2(
                            this.Center.X - outRadius * (float)Math.Cos(angle),
                            this.Center.Y - outRadius * (float)Math.Sin(angle));
                        this.Points.Add(point);
                    }
                    for (var i = 0; i <= this._quality; i++)
                    {
                        var angle = i * 2 * Math.PI / this._quality;
                        var point = new Vector2(
                            this.Center.X + innerRadius * (float)Math.Cos(angle),
                            this.Center.Y - innerRadius * (float)Math.Sin(angle));
                        this.Points.Add(point);
                    }
                }

                #endregion
            }

            /// <summary>
            ///     Represnets a sector polygon.
            /// </summary>
            public class Sector : Polygon
            {
                #region Fields

                /// <summary>
                ///     The angle
                /// </summary>
                public float Angle;

                /// <summary>
                ///     The center
                /// </summary>
                public Vector2 Center;

                /// <summary>
                ///     The direction
                /// </summary>
                public Vector2 Direction;

                /// <summary>
                ///     The radius
                /// </summary>
                public float Radius;

                /// <summary>
                ///     The quality
                /// </summary>
                private readonly int _quality;

                #endregion

                #region Constructors and Destructors

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Sector" /> class.
                /// </summary>
                /// <param name="center">The center.</param>
                /// <param name="direction">The direction.</param>
                /// <param name="angle">The angle.</param>
                /// <param name="radius">The radius.</param>
                /// <param name="quality">The quality.</param>
                public Sector(Vector3 center, Vector3 direction, float angle, float radius, int quality = 20)
                    : this(center.ToVector2(), direction.ToVector2(), angle, radius, quality)
                {
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Polygon.Sector" /> class.
                /// </summary>
                /// <param name="center">The center.</param>
                /// <param name="direction">The direction.</param>
                /// <param name="angle">The angle.</param>
                /// <param name="radius">The radius.</param>
                /// <param name="quality">The quality.</param>
                public Sector(Vector2 center, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    this.Center = center;
                    this.Direction = (direction - center).Normalized();
                    this.Angle = angle;
                    this.Radius = radius;
                    this._quality = quality;
                    this.UpdatePolygon();
                }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Rotates Line by angle/radian
                /// </summary>
                /// <param name="point1"></param>
                /// <param name="point2"></param>
                /// <param name="value"></param>
                /// <param name="radian">True for radian values, false for degree</param>
                /// <returns></returns>
                public Vector2 RotateLineFromPoint(Vector2 point1, Vector2 point2, float value, bool radian = true)
                {
                    var angle = !radian ? value * Math.PI / 180 : value;
                    var line = Vector2.Subtract(point2, point1);

                    var newline = new Vector2
                    {
                        X = (float)(line.X * Math.Cos(angle) - line.Y * Math.Sin(angle)),
                        Y = (float)(line.X * Math.Sin(angle) + line.Y * Math.Cos(angle))
                    };

                    return Vector2.Add(newline, point1);
                }

                /// <summary>
                ///     Updates the polygon.
                /// </summary>
                /// <param name="offset">The offset.</param>
                public void UpdatePolygon(int offset = 0)
                {
                    this.Points.Clear();
                    var outRadius = (this.Radius + offset) / (float)Math.Cos(2 * Math.PI / this._quality);
                    this.Points.Add(this.Center);
                    var side1 = this.Direction.Rotated(-this.Angle * 0.5f);
                    for (var i = 0; i <= this._quality; i++)
                    {
                        var cDirection = side1.Rotated(i * this.Angle / this._quality).Normalized();
                        this.Points.Add(
                            new Vector2(
                                this.Center.X + outRadius * cDirection.X,
                                this.Center.Y + outRadius * cDirection.Y));
                    }
                }

                #endregion
            }
        }
    }
}