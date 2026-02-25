using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Numerics;

namespace AdventOfCode.Y2024.Day12;

[ProblemName("Garden Groups")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, CalcEdges);
    public object PartTwo(string input) => Solve(input, CalcCorners);

    /// <summary>
    /// Core logic that identifies regions and sums their (Area * Metric).
    /// </summary>
    private static long Solve(string input, Func<HashSet<Vector2Int>, int> calc) {
        List<HashSet<Vector2Int>> regions = FindRegions(input);
        return regions.Sum(region => (long)region.Count * calc(region));
    }
    
    /// <summary>
    /// Calculates the standard perimeter by checking how many neighbors are NOT in the region.
    /// </summary>
    private static int CalcEdges(HashSet<Vector2Int> region) {
        int perimeter = 0;
        Vector2Int[] directions = [Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right];

        foreach (Vector2Int pos in region) {
            foreach (Vector2Int dir in directions) {
                if (!region.Contains(pos + dir)) perimeter++;
            }
        }
        return perimeter;
    }

    /// <summary>
    /// Calculates sides by counting corners. Each corner (convex or concave) 
    /// represents the start of a new side.
    /// </summary>
    private static int CalcCorners(HashSet<Vector2Int> region) {
        int corners = 0;
        (Vector2Int, Vector2Int)[] diagonalPairs =
        [
            (Vector2Int.Up, Vector2Int.Right),
            (Vector2Int.Right, Vector2Int.Down),
            (Vector2Int.Down, Vector2Int.Left),
            (Vector2Int.Left, Vector2Int.Up)
        ];

        foreach (Vector2Int p in region) {
            foreach ((Vector2Int a, Vector2Int b) in diagonalPairs) {
                bool hasA = region.Contains(p + a);
                bool hasB = region.Contains(p + b);
                bool hasDiagonal = region.Contains(p + a + b);

                switch (hasA)
                {
                    // Convex
                    case false when !hasB:
                    // Concave
                    case true when hasB && !hasDiagonal:
                        corners++;
                        break;
                }
            }
        }
        return corners;
    }

    /// <summary>
    /// Identifies distinct connected regions of points from the input grid.
    /// Each region consists of adjacent cells marked as part of the same group.
    /// </summary>
    private static List<HashSet<Vector2Int>> FindRegions(string input)
    {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        HashSet<Vector2Int> visited = [];
        List<HashSet<Vector2Int>> regions = [];

        foreach (Vector2Int pos in GetGridPoints(lines)) {
            if (visited.Contains(pos)) continue;
            regions.Add(FloodFill(pos, lines, visited));
        }

        return regions;
    }

    /// <summary>
    /// BFS to aggregate all connected points of the same type.
    /// </summary>
    private static HashSet<Vector2Int> FloodFill(Vector2Int start, string[] lines, HashSet<Vector2Int> visited) {
        HashSet<Vector2Int> region = [];
        Queue<Vector2Int> queue = new();
        char target = lines[start.y][start.x];

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.TryDequeue(out Vector2Int c)) {
            region.Add(c);

            foreach (Vector2Int neighbor in GetNeighbors(c, target, lines, visited)) {
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
        return region;
    }

    /// <summary>
    /// Helper to find adjacent points that match the target and are within bounds.
    /// </summary>
    private static IEnumerable<Vector2Int> GetNeighbors(Vector2Int curr, char target, string[] lines, HashSet<Vector2Int> visited) {
        Vector2Int[] directions = [Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right];
        int rows = lines.Length;
        int cols = lines[0].Length;

        foreach (Vector2Int dir in directions) {
            Vector2Int next = curr + dir;

            if (next.x >= 0 && next.x < cols && next.y >= 0 && next.y < rows &&
                !visited.Contains(next) && 
                lines[next.y][next.x] == target) {
                yield return next;
            }
        }
    }

    /// <summary>
    /// Helper to iterate through a 2D grid
    /// </summary>
    private static IEnumerable<Vector2Int> GetGridPoints(string[] lines)
    {
        for (int y = 0; y < lines.Length; y++) {
            for (int x = 0; x < lines[y].Length; x++) {
                yield return new Vector2Int(x, y);
            }
        }
    }
}