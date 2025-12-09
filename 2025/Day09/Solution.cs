namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Movie Theater")]
class Solution : Solver
{
    // ReSharper disable InconsistentNaming
    private readonly record struct Rect(int aX, int aY, int bX, int bY)
    {
        public long Area { get; init; } = (long)(bX - aX + 1) * (bY - aY + 1);
    }
    private readonly record struct V2(int X, int Y);
    // ReSharper restore InconsistentNaming
    
    public object PartOne(string input)
    {
        V2[] points = ParsePoints(input);

        return ParseRects(points)
            .OrderByDescending(r => r.Area)
            .FirstOrDefault().Area;
    }

    public object PartTwo(string input)
    {
        V2[] points = ParsePoints(input);
        Rect[] boundaries = ParseBoundaries(points);

        return ParseRects(points)
            .Where(r => boundaries.All(b => !AABB(r, b)))
            .OrderByDescending(r => r.Area)
            .FirstOrDefault().Area;
    }

    private static V2[] ParsePoints(string input) {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        V2[] points = new V2[lines.Length];
        for (int i = 0; i < lines.Length; i++) {
            string[] parts = lines[i].Split(',');
            points[i] = new V2(int.Parse(parts[0]), int.Parse(parts[1]));
        }
        return points;
    }

    private static IEnumerable<Rect> ParseRects(V2[] points)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            for (int j = i + 1; j < points.Length; j++)
            {
                yield return ParseRect(points[i], points[j]);
            }
        }
    }
    
    private static Rect ParseRect(V2 a, V2 b) =>
        new Rect(
            Math.Min(a.X, b.X),
            Math.Min(a.Y, b.Y),
            Math.Max(a.X, b.X),
            Math.Max(a.Y, b.Y));

    /// <summary>
    /// Converts a closed polyline into a set of boundary rectangles
    /// These rectangles use inclusive coordinates to match <see cref="AABB(Rect, Rect)"/>.
    /// </summary>
    private static Rect[] ParseBoundaries(V2[] points) {
        int pointCount = points.Length;
        Rect[] boundaries = new Rect[pointCount];
        for (int i = 0; i < pointCount; i++) {
            V2 p1 = points[i];
            V2 p2 = points[(i + 1) % pointCount];
            boundaries[i] = ParseRect(p1, p2);
        }
        return boundaries;
    }
    
    /// <summary>
    /// Tests whether two rectangles overlap using inclusive bounds.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private static bool AABB(Rect a, Rect b) =>
        a.bX > b.aX &&
        a.aX < b.bX &&
        a.bY > b.aY &&
        a.aY < b.bY;
}