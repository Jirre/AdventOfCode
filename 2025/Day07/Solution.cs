namespace AdventOfCode.Y2025.Day07;

using System.Collections.Generic;
using System.Linq;

[ProblemName("Laboratories")]
class Solution : Solver {

    #region --- Part 1 ---

    public object PartOne(string input)
    {
        string[] grid = ParseGrid(input);
        (int startRow, int startCol) = FindStart(grid);

        return CountSplits(grid, startRow, startCol);
    }
    
    private static int CountSplits(string[] grid, int startRow, int startCol)
    {
        int splitCount = 0;
        Queue<(int r, int c)> queue = new Queue<(int r, int c)>();
        queue.Enqueue((startRow, startCol));

        HashSet<(int r, int c)> visited = new HashSet<(int r, int c)>();

        while (queue.Count > 0)
        {
            (int r, int c) = queue.Dequeue();

            (int row, bool hit) = Step(grid, r, c, visited);

            if (!hit) continue;
            splitCount++;
            EnqueueSplitter(queue, row, c, grid[0].Length);
        }

        return splitCount;
    }
    
    private static void EnqueueSplitter(Queue<(int r, int c)> queue, int r, int c, int width)
    {
        if (c - 1 >= 0) queue.Enqueue((r, c - 1));
        if (c + 1 < width) queue.Enqueue((r, c + 1));
    }

    #endregion

    #region --- Part 2 ---

    public object PartTwo(string input)
    {
        string[] grid = ParseGrid(input);
        (int startRow, int startCol) = FindStart(grid);
        long timelines = CountUniqueRoutes(grid, startRow, startCol);
        return timelines;
    }
    
    private static long CountUniqueRoutes(string[] grid, int startRow, int startCol)
    {
        int width = grid[0].Length;

        Queue<(int r, int c)> queue = new Queue<(int r, int c)>();
        
        // Multiple paths might go through this node, the paths from this node will be identical
        // No need to calculate multiple times
        Dictionary<(int r, int c), long> nodeRoutes  = new Dictionary<(int r, int c), long>();

        // Begin a route at the start position
        AddRoute(nodeRoutes, queue, startRow, startCol, 1, width);

        long totalTimelines = 0;

        while (queue.Count > 0)
        {
            (int r, int c) = queue.Dequeue();
            long count = nodeRoutes[(r, c)];
            nodeRoutes.Remove((r, c));

            (int row, bool hitSplitter) = Step(grid, r, c, new HashSet<(int r, int c)>());

            if (!hitSplitter)
            {
                // Reached the end of the grid
                totalTimelines += count;
                continue;
            }

            // Hit a splitter create two new routes
            AddRoute(nodeRoutes, queue, row, c - 1, count, width);
            AddRoute(nodeRoutes, queue, row, c + 1, count, width);
        }

        return totalTimelines;
    }
    
    private static void AddRoute(
        Dictionary<(int r, int c), long> nodeCrossings,
        Queue<(int r, int c)> queue,
        int r,
        int c,
        long count,
        int width)
    {
        if (c < 0 || c >= width) return;

        (int r, int c) key = (r, c);
        if (nodeCrossings.TryGetValue(key, out long crossings))
            nodeCrossings[key] = crossings + count;
        else
        {
            nodeCrossings[key] = count;
            queue.Enqueue(key);
        }
    }

    #endregion
    
    private static string[] ParseGrid(string input) => 
        input.Split('\n').Select(r => r.TrimEnd('\r')).ToArray();
    
    private static (int row, int col) FindStart(string[] grid) =>
        (0, grid[0].IndexOf('S'));
    
    private static (int row, bool hitSplitter) Step(string[] grid, int r, int c, HashSet<(int r, int c)> visited)
    {
        int height = grid.Length;
        int width = grid[0].Length;

        while (true)
        {
            r++;

            if (r < 0 || r >= height ||
                c < 0 || c >= width || 
                visited != null && !visited.Add((r, c)))
                return (r, false);

            if (grid[r][c] == '^')
                return (r, true);
        }
    }
}