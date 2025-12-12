namespace AdventOfCode.Y2024.Day06;

using System.Collections.Generic;
using System.Linq;
using Shared.Numerics;

[ProblemName("Guard Gallivant")]
class Solution : Solver
{
    private readonly record struct Map(Dictionary<Vector2Int, char> Nodes, Vector2Int Start);
    private readonly record struct Route(IEnumerable<Vector2Int> Positions, bool IsLoop);
    private readonly record struct Step(Vector2Int Position,  Vector2Int Direction);

    public object PartOne(string input)
    {
        Map map = ParseMap(input);
        return Walk(map.Nodes, map.Start).Positions.Count();
    }

    public object PartTwo(string input)
    {
        Map map = ParseMap(input);
        return Walk(map.Nodes, map.Start).Positions
            .AsParallel()
            .Count(pos => Walk(AddObstacle(map.Nodes, pos), map.Start).IsLoop);
    }

    private static Route Walk(Dictionary<Vector2Int, char> map, Vector2Int start)
    {
        HashSet<Step> seen = new();
        Vector2Int pos = start;
        Vector2Int dir = Vector2Int.Up;

        while (map.ContainsKey(pos) && !seen.Contains(new Step(pos, dir)))
        {
            seen.Add(new Step(pos, dir));

            if (map.TryGetValue(pos + dir, out char next) && next == '#')
                dir = dir.Rotate(90);
            else
                pos += dir;
        }

        bool isLoop = seen.Contains(new Step(pos, dir));
        return new Route(seen.Select(s => s.Position).Distinct(), isLoop);
    }

    private static Dictionary<Vector2Int, char> AddObstacle(Dictionary<Vector2Int, char> map, Vector2Int pos)
    {
        Dictionary<Vector2Int, char> copy = new (map)
        {
            [pos] = '#'
        };
        return copy;
    }

    private static Map ParseMap(string input)
    {
        string[] lines = input.Split('\n');
        Dictionary<Vector2Int, char> nodes = new Dictionary<Vector2Int, char>();

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                nodes[new Vector2Int(x, lines.Length - 1 - y)] = lines[y][x];
            }
        }

        Vector2Int start = nodes.First(kvp => kvp.Value == '^').Key;
        return new Map(nodes, start);
    }
}
