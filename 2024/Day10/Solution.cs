namespace AdventOfCode.Y2024.Day10;

using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Numerics;

[ProblemName("Hoof It")]
class Solution : Solver {

    public object PartOne(string input) {
        Dictionary<Vector2Int, List<Vector2Int>> routes = ParseRoutes(input);
        return routes.Sum(r => r.Value.Distinct().Count());
    }

    public object PartTwo(string input) {
        Dictionary<Vector2Int, List<Vector2Int>> routes = ParseRoutes(input);
        return routes.Sum(r => r.Value.Count);
    }

    private static Dictionary<Vector2Int, char> ParseMap(string input) {
        string[] rows = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Dictionary<Vector2Int, char> map = new Dictionary<Vector2Int, char>(rows.Length * rows[0].Length);
        for (int y = 0; y < rows.Length; y++) {
            int invY = -y;
            for (int x = 0; x < rows[0].Length; x++) {
                map[new Vector2Int(x, invY)] = rows[y][x];
            }
        }
        return map;
    }
    
    private Dictionary<Vector2Int, List<Vector2Int>> ParseRoutes(string input) {
        Dictionary<Vector2Int, char> map = ParseMap(input);
        IEnumerable<Vector2Int> trailHeads = GetStarts(map);
        Dictionary<Vector2Int, List<Vector2Int>> result = new Dictionary<Vector2Int, List<Vector2Int>>();
        foreach (Vector2Int head in trailHeads) {
            result[head] = Pathfind(map, head);
        }
        return result;
    }

    private static IEnumerable<Vector2Int> GetStarts(Dictionary<Vector2Int, char> map) {
        foreach (KeyValuePair<Vector2Int, char> kv in map) {
            if (kv.Value == '0') yield return kv.Key;
        }
    }

    private static List<Vector2Int> Pathfind(Dictionary<Vector2Int, char> map, Vector2Int trailHead) {
        Queue<Vector2Int> positions = new Queue<Vector2Int>();
        positions.Enqueue(trailHead);
        List<Vector2Int> trails = new List<Vector2Int>();

        while (positions.Count > 0) {
            Vector2Int point = positions.Dequeue();
            if (map[point] == '9') {
                trails.Add(point);
            } else {
                foreach (Vector2Int dir in new[] { Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right }) {
                    Vector2Int next = point + dir;
                    if (map.TryGetValue(next, out char val) && val == map[point] + 1) {
                        positions.Enqueue(next);
                    }
                }
            }
        }

        return trails;
    }
}