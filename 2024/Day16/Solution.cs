using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Numerics;

namespace AdventOfCode.Y2024.Day16;

[ProblemName("Reindeer Maze")]
class Solution : Solver {
    
    private record Node(Vector2Int Pos, Vector2Int Dir);
    
    public object PartOne(string input) {
        (char[][] grid, Vector2Int start, Vector2Int goal) = Parse(input);
        Dictionary<Node, int> distances = Dijkstra(grid, goal);
        return distances[new Node(start, Vector2Int.Right)];
    }

    public object PartTwo(string input) {
        (char[][] grid, Vector2Int start, Vector2Int goal) = Parse(input);
        Dictionary<Node, int> distances = Dijkstra(grid, goal);
        
        Node startNode = new Node(start, Vector2Int.Right);
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(startNode);

        HashSet<Vector2Int> bestSpots = [start];
        HashSet<Node> visited = [startNode];

        while (queue.TryDequeue(out Node current)) {
            int currentRemaining = distances[current];

            foreach ((Node next, int stepCost) in GetNeighbors(grid, current, forward: true))
            {
                // If this neighbor's cost matches exactly what we expect for a shortest path
                if (!distances.TryGetValue(next, out int nextRemaining) ||
                    nextRemaining != currentRemaining - stepCost) continue;
                
                bestSpots.Add(next.Pos);
                if (visited.Add(next)) {
                    queue.Enqueue(next);
                }
            }
        }
        return bestSpots.Count;
    }

    private static Dictionary<Node, int> Dijkstra(char[][] grid, Vector2Int goal) {
        Dictionary<Node, int> distances = new Dictionary<Node, int>();
        PriorityQueue<Node, int> queue = new PriorityQueue<Node, int>();

        // Backward Dijkstra: Start from the goal facing all directions with 0 cost
        foreach (Vector2Int d in new[] { Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right }) {
            Node node = new Node(goal, d);
            distances[node] = 0;
            queue.Enqueue(node, 0);
        }

        while (queue.TryDequeue(out Node curr, out int cost)) {
            if (cost > distances[curr]) continue;

            foreach ((Node prev, int stepCost) in GetNeighbors(grid, curr, forward: false)) {
                int newCost = cost + stepCost;
                if (newCost >= distances.GetValueOrDefault(prev, int.MaxValue)) continue;
                distances[prev] = newCost;
                queue.Enqueue(prev, newCost);
            }
        }
        return distances;
    }

    private static IEnumerable<(Node Node, int Cost)> GetNeighbors(char[][] grid, Node node, bool forward) {
        Vector2Int moveDir = forward ? node.Dir : node.Dir * -1;
        Vector2Int nextPos = node.Pos + moveDir;
        if (grid[nextPos.y][nextPos.x] != '#') {
            yield return (new Node(nextPos, node.Dir), 1);
        }
        
        yield return (node with {Dir = node.Dir.Rotate(90)}, 1000);
        yield return (node with {Dir = node.Dir.Rotate(270)}, 1000);
    }

    private static (char[][] grid, Vector2Int start, Vector2Int goal) Parse(string input) {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        char[][] grid = lines.Select(l => l.ToCharArray()).ToArray();
        Vector2Int s = default, g = default;

        for (int y = 0; y < grid.Length; y++) {
            for (int x = 0; x < grid[y].Length; x++) {
                if (grid[y][x] == 'S') s = new Vector2Int(x, y);
                if (grid[y][x] == 'E') g = new Vector2Int(x, y);
            }
        }
        return (grid, s, g);
    }
}