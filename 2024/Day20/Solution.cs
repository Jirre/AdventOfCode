using Shared.Numerics;

namespace AdventOfCode.Y2024.Day20;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ProblemName("Race Condition")]
class Solution : Solver
{
    private record Map(bool[,] IsWall, Vector2Int Start, Vector2Int Goal);
    
    public object PartOne(string input) => Solve(input, 2);
    public object PartTwo(string input) => Solve(input, 20);

    private static int Solve(string input, int maxCheatDist) {
        Vector2Int[] path = GetPath(input);
        int totalWorthyCheats = 0;
        object lockObj = new();
        
        Parallel.For(0, path.Length, () => 0, (i, _, localSum) => {
            for (int j = i + 102; j < path.Length; j++) {
                int dist = path[i].ManhattanDistance(path[j]);

                if (dist > maxCheatDist) continue;
                int saving = (j - i) - dist;
                if (saving >= 100) {
                    localSum++;
                }
            }
            return localSum;
        }, 
        localSum => { 
            lock (lockObj) totalWorthyCheats += localSum; 
        });

        return totalWorthyCheats;
    }

    private static Vector2Int[] GetPath(string input) {
        Map map = ParseMap(input);
        return ProcessPath(map);
    }

    /// <summary>
    /// Parses the input string into a map structure, identifying walls, start, and goal.
    /// </summary>
    /// <param name="input">A string representation of the map, where each line defines a row.
    /// '#' represents walls, 'S' represents the starting point, and 'E' represents the goal point.</param>
    /// <returns>A <see cref="Map"/> object containing the wall matrix, starting position, and goal position.</returns>
    private static Map ParseMap(string input)
    {
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        int width = lines[0].Length, height = lines.Length;

        bool[,] isWall = new bool[width, height];
        Vector2Int start = default, goal = default;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                char cell = lines[y][x];
                isWall[x, y] = cell == '#';
                switch (cell)
                {
                    case 'S':
                        start = new Vector2Int(x, y);
                        break;
                    case 'E':
                        goal = new Vector2Int(x, y);
                        break;
                }
            }
        }
        return new Map(isWall, start, goal);
    }

    /// <summary>
    /// Processes a navigable path through a map and determines the sequence of positions
    /// from the starting point to the goal point.
    /// </summary>
    /// <param name="map">The map containing walls, the starting point, and the goal point.</param>
    /// <returns>An array of <see cref="Vector2Int"/> representing the path from the start to the goal.</returns>
    private static Vector2Int[] ProcessPath(Map map)
    {
        List<Vector2Int> path = [map.Start];
        Vector2Int current = map.Start;
        Vector2Int? previous = null;

        while (current != map.Goal) {
            Vector2Int next = Step(current, previous, map.IsWall);
            path.Add(next);
            previous = current;
            current = next;
        }

        return path.ToArray();
    }

    /// <summary>
    /// Determines the next valid step in a pathfinding process, avoiding walls and the previously visited position.
    /// </summary>
    /// <param name="current">The current position in the grid.</param>
    /// <param name="previous">The previous position in the grid, or null if there is no prior position.</param>
    /// <param name="isWall">A 2D boolean grid indicating wall locations where true represents a wall.</param>
    /// <returns>The next valid position as a <see cref="Vector2Int"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no valid step can be found, indicating a broken path or a dead end.</exception>
    private static Vector2Int Step(Vector2Int current, Vector2Int? previous, bool[,] isWall)
    {
        ReadOnlySpan<Vector2Int> directions =
        [
            Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right
        ];

        foreach (Vector2Int dir in directions) {
            Vector2Int neighbor = current + dir;
            if (!isWall[neighbor.x, neighbor.y] && neighbor != previous) {
                return neighbor;
            }
        }

        throw new InvalidOperationException("The track is broken or has a dead end.");
    }
}