using System.Linq;

namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;

[ProblemName("Movie Theater")]
class Solution : Solver
{
    private readonly record struct Rect(int X1, int Y1, int X2, int Y2);
    private readonly record struct V2(int X, int Y);
    
    public object PartOne(string input)
    {
        V2[] points = ParsePoints(input);

        return ParseAndOrderRects(points)
            .Select(GetArea)
            .FirstOrDefault();
    }

    public object PartTwo(string input)
    {
        V2[] points = ParsePoints(input);
        Rect[] boundaries = ParseBoundaries(points);

        return ParseAndOrderRects(points)
            .Where(r => boundaries.All(b => !AABB(r, b)))
            .Select(GetArea)
            .FirstOrDefault();
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

    private static Rect ParseRect(V2 p1, V2 p2) {
        int x1 = Math.Min(p1.X, p2.X);
        int x2 = Math.Max(p1.X, p2.X);
        int y1 = Math.Min(p1.Y, p2.Y);
        int y2 = Math.Max(p1.Y, p2.Y);
        return new Rect(x1, y1, x2, y2);
    }
    
    /// <summary>
    /// Generate, order, and loop over the rects of all provided points
    /// </summary>
    /// <returns>An enumeration of rectangles sorted by decreasing area.</returns>
    private static IEnumerable<Rect> ParseAndOrderRects(V2[] points) {
        List<Rect> rects = [];
        foreach (V2 p in points)
        {
            foreach (V2 q in points)
            {
                rects.Add(ParseRect(p, q));
            }
        }

        rects.Sort((r1, r2) => GetArea(r2).CompareTo(GetArea(r1)));
        foreach (Rect r in rects) yield return r;
    }
    
    private static long GetArea(Rect r) => (long)(r.X2 - r.X1 + 1) * (r.Y2 - r.Y1 + 1);

    /// <summary>
    /// Converts a closed polyline into a set of boundary rectangles
    /// These rectangles use inclusive coordinates to match <see cref="GetArea(Rect)"/> and <see cref="AABB(Rect, Rect)"/>.
    /// </summary>
    private static Rect[] ParseBoundaries(V2[] corners) {
        int n = corners.Length;
        Rect[] boundaries = new Rect[n];
        for (int i = 0; i < n; i++) {
            V2 p1 = corners[i];
            V2 p2 = corners[(i + 1) % n];
            boundaries[i] = ParseRect(p1, p2);
        }
        return boundaries;
    }
    
    /// <summary>
    /// Tests whether two rectangles overlap using inclusive bounds.
    /// </summary>
    private static bool AABB(Rect a, Rect b) {
        bool aLeft = a.X2 <= b.X1;
        bool aRight = a.X1 >= b.X2;
        bool aAbove = a.Y2 <= b.Y1;
        bool aBelow = a.Y1 >= b.Y2;
        return !(aLeft || aRight || aAbove || aBelow);
    }
}
