using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Numerics;

namespace AdventOfCode.Y2024.Day18;

[ProblemName("RAM Run")]
class Solution : Solver {
    private const int GRID_SIZE = 70;

    public object PartOne(string input) {
        Vector2Int[] blocks = ParseCoordinates(input);
        return FindShortestPath(blocks.Take(1024), GRID_SIZE);
    }

    public object PartTwo(string input) {
        Vector2Int[] blocks = ParseCoordinates(input);
        
        int low = 0;
        int high = blocks.Length - 1;
        int resultIndex = -1;

        // Binary search to find the exact index of the block that breaks the path
        while (low <= high) {
            int mid = low + (high - low) / 2;
            
            if (FindShortestPath(blocks.Take(mid + 1), GRID_SIZE) == null) {
                resultIndex = mid; // Continue search for an earlier one
                high = mid - 1;
            } else {
                low = mid + 1;
            }
        }

        Vector2Int resultBlock = blocks[resultIndex];
        return $"{resultBlock.x},{resultBlock.y}";
    }

    private static int? FindShortestPath(IEnumerable<Vector2Int> obstacles, int size) {
        Vector2Int start = Vector2Int.Zero;
        Vector2Int goal = new Vector2Int(size, size);
        HashSet<Vector2Int> occupied = [..obstacles, start];

        Queue<(Vector2Int Pos, int Dist)> queue = new Queue<(Vector2Int Pos, int Dist)>();
        queue.Enqueue((start, 0));
        
        ReadOnlySpan<Vector2Int> directions = [
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
        ];

        while (queue.TryDequeue(out var current)) {
            if (current.Pos == goal) return current.Dist;

            foreach (Vector2Int dir in directions) {
                Vector2Int next = current.Pos + dir;

                if (IsInsideGrid(next, size) && occupied.Add(next)) {
                    queue.Enqueue((next, current.Dist + 1));
                }
            }
        }
        return null;
    }

    private static bool IsInsideGrid(Vector2Int p, int size) => 
        p.x >= 0 && p.x <= size && p.y >= 0 && p.y <= size;

    private static Vector2Int[] ParseCoordinates(string input) =>
        Regex.Matches(input, @"(\d+),(\d+)")
            .Select(m => new Vector2Int(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)))
            .ToArray();
}