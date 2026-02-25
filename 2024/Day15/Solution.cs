using System.Collections.Generic;
using System.Linq;
using Shared.Numerics;

namespace AdventOfCode.Y2024.Day15;

[ProblemName("Warehouse Woes")]
class Solution : Solver {
    public object PartOne(string input) => Solve(input);
    public object PartTwo(string input) => Solve(ScaleUp(input));
    private static string ScaleUp(string input) =>
        input.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");

    private static long Solve(string input) {
        (char[][] grid, Vector2Int[] moves, Vector2Int robot) = Parse(input);

        foreach (Vector2Int dir in moves)
        {
            if (!TryPush(grid, robot, dir, out List<Vector2Int> affectedNodes)) continue;
            ApplyMove(grid, affectedNodes, dir);
            robot += dir;
        }

        return CalcSum(grid);
    }

    private static bool TryPush(char[][] grid, Vector2Int start, Vector2Int dir, out List<Vector2Int> affected) {
        affected = [start];
        HashSet<Vector2Int> seen = [start];

        for (int i = 0; i < affected.Count; i++) {
            Vector2Int next = affected[i] + dir;
            char cell = grid[next.y][next.x];

            switch (cell)
            {
                case '#':
                    return false; // Immediate failure on wall
                case '.':
                    continue;     // Empty space is a no-op
            }
            
            if (!seen.Add(next)) continue;
            affected.Add(next);
            AddBoxDependencies(next, cell, dir, seen, affected);
        }
        return true;
    }

    private static void AddBoxDependencies(
        Vector2Int pos, char cell, Vector2Int dir, HashSet<Vector2Int> seen, List<Vector2Int> affected
    ) {
        if (dir.y == 0) return;

        Vector2Int otherHalf = cell switch {
            '[' => pos + Vector2Int.Right,
            ']' => pos + Vector2Int.Left,
            _   => pos
        };

        if (otherHalf != pos && seen.Add(otherHalf)) {
            affected.Add(otherHalf);
        }
    }

    private static void ApplyMove(char[][] grid, List<Vector2Int> nodes, Vector2Int dir) {
        List<(Vector2Int Pos, char Val)> changes = nodes.Select(p => (Pos: p, Val: grid[p.y][p.x])).ToList();
        foreach ((Vector2Int Pos, char Val) c in changes) grid[c.Pos.y][c.Pos.x] = '.';
        foreach ((Vector2Int Pos, char Val) c in changes) grid[c.Pos.y + dir.y][c.Pos.x + dir.x] = c.Val;
    }

    private static long CalcSum(char[][] grid) => (
        from y in Enumerable.Range(0, grid.Length)
        from x in Enumerable.Range(0, grid[y].Length)
        where grid[y][x] is 'O' or '['
        select (long)(100 * y + x)
    ).Sum();

    private static (char[][] grid, Vector2Int[] moves, Vector2Int robot) Parse(string input) {
        string[] blocks = input.Split("\n\n");
        char[][] grid = blocks[0].Split('\n').Select(l => l.ToCharArray()).ToArray();
        
        Vector2Int[] moves = blocks[1].Where(c => "^v<>".Contains(c)).Select(c => c switch {
            '^' => new Vector2Int(0, -1),
            'v' => new Vector2Int(0, 1),
            '<' => new Vector2Int(-1, 0),
            '>' => new Vector2Int(1, 0),
            _ => Vector2Int.Zero
        }).ToArray();

        Vector2Int robot = Enumerable.Range(0, grid.Length)
            .SelectMany(y => Enumerable.Range(0, grid[y].Length).Select(x => new Vector2Int(x, y)))
            .First(p => grid[p.y][p.x] == '@');

        return (grid, moves, robot);
    }
}