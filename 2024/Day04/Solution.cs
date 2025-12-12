using System.Linq;

namespace AdventOfCode.Y2024.Day04;

using System.Collections.Generic;
using Shared.Numerics;

[ProblemName("Ceres Search")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        Dictionary<Vector2Int, char> map = ParseMap(input);
        Vector2Int[] directions = new[]
        {
            Vector2Int.Right,
            Vector2Int.Right + Vector2Int.Down,
            Vector2Int.Down + Vector2Int.Left,
            Vector2Int.Down
        };

        return map.Keys.Sum(pt => directions.Count(dir => Matches(map, pt, dir, "XMAS")));
    }

    public object PartTwo(string input)
    {
        Dictionary<Vector2Int, char> map = ParseMap(input);
        return map.Keys.Count(pt =>
            Matches(map, pt + Vector2Int.Up + Vector2Int.Left, Vector2Int.Down + Vector2Int.Right, "MAS") &&
            Matches(map, pt + Vector2Int.Down + Vector2Int.Left, Vector2Int.Up + Vector2Int.Right, "MAS")
        );
    }
    
    private static Dictionary<Vector2Int, char> ParseMap(string input)
    {
        string[] lines = input.Split('\n');
        Dictionary<Vector2Int, char> map = new Dictionary<Vector2Int, char>();

        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                map[new Vector2Int(x, y)] = line[x];
            }
        }

        return map;
    }

    private static bool Matches(Dictionary<Vector2Int, char> map, Vector2Int pt, Vector2Int dir, string pattern) =>
        MatchesForward(map, pt, dir, pattern) || 
        MatchesReverse(map, pt, dir, pattern);

    private static bool MatchesForward(Dictionary<Vector2Int, char> map, Vector2Int pt, Vector2Int dir, string pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            if (!map.TryGetValue(pt + dir * i, out char c) ||
                c != pattern[i])
                return false;
        }
        return true;
    }

    private static bool MatchesReverse(Dictionary<Vector2Int, char> map, Vector2Int pt, Vector2Int dir, string pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            if (!map.TryGetValue(pt + dir * i, out char c) || c != pattern[pattern.Length - 1 - i])
                return false;
        }
        return true;
    }
}
