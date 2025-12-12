namespace AdventOfCode.Y2024.Day08;

using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Numerics;

[ProblemName("Resonant Collinearity")]
class Solution : Solver
{
    public object PartOne(string input) => GetPositions(input, GetPointOverlaps).Count;
    public object PartTwo(string input) => GetPositions(input, GetLineOverlaps).Count;

    private static Dictionary<Vector2Int, char> ParseMap(string input)
    {
        string[] lines = input.Split('\n');
        Dictionary<Vector2Int, char> map = new();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[new Vector2Int(x, lines.Length - 1 - y)] = lines[y][x];
            }
        }

        return map;
    }
    
    private static HashSet<Vector2Int> GetPositions(string input, Func<Vector2Int, Vector2Int, Dictionary<Vector2Int, char>, IEnumerable<Vector2Int>> getOverlaps)
    {
        Dictionary<Vector2Int, char> map = ParseMap(input);

        Vector2Int[] mapPoints = map
            .Where(kvp => kvp.Value != '.')
            .Select(kvp => kvp.Key)
            .ToArray();

        HashSet<Vector2Int> positions = new();

        foreach (Vector2Int source in mapPoints)
        {
            foreach (Vector2Int target in mapPoints)
            {
                if (source == target || map[source] != map[target])
                    continue;

                foreach (Vector2Int overlap in getOverlaps(source, target, map))
                    positions.Add(overlap);
            }
        }

        return positions;
    }
    
    private static IEnumerable<Vector2Int> GetPointOverlaps(Vector2Int source, Vector2Int target, Dictionary<Vector2Int, char> map)
    {
        Vector2Int direction = target - source;
        Vector2Int overlap = target + direction;
        if (map.ContainsKey(overlap))
            yield return overlap;
    }
    
    private static IEnumerable<Vector2Int> GetLineOverlaps(Vector2Int source, Vector2Int target, Dictionary<Vector2Int, char> map)
    {
        Vector2Int direction = target - source;
        Vector2Int position = target;
        while (map.ContainsKey(position))
        {
            yield return position;
            position += direction;
        }
    }
}
