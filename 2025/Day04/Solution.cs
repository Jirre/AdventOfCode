namespace AdventOfCode.Y2025.Day04;

using System;
using System.Collections.Generic;

[ProblemName("Printing Department")]
class Solution : Solver
{
    private readonly record struct V2(int X, int Y);

    private static readonly V2[] DIRS = new V2[]
    {
        new(-1,-1), new(0,-1), new(1,-1),
        new(-1, 0),            new(1, 0),
        new(-1, 1), new(0, 1), new(1, 1)
    };

    public object PartOne(string input)
    {
        char[,] grid = ParseGrid(input.AsSpan(), out int width, out int height);
        return CountAccessible(grid, width, height);
    }

    public object PartTwo(string input)
    {
        char[,] grid = ParseGrid(input.AsSpan(), out int width, out int height);
        return RemoveAccessible(grid, width, height);
    }
    
    private static char[,] ParseGrid(ReadOnlySpan<char> span, out int width, out int height)
    {
        width = span.IndexOf('\n');
        if (width < 0) width = span.Length;

        height = 1;
        foreach (char t in span)
            if (t == '\n') height++;

        char[,] grid = new char[height, width];

        int x = 0, y = 0;
        foreach (char ch in span)
        {
            if (ch == '\n')
            {
                y++;
                x = 0;
                continue;
            }
            if (x < width) grid[y, x] = ch;
            x++;
        }

        return grid;
    }
    
    private static int CountAccessible(char[,] grid, int width, int height)
    {
        int accessible = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y, x] != '@') continue;
                if (CountNeighbors(grid, x, y, width, height) < 4)
                    accessible++;
            }
        }
        return accessible;
    }
    
    private static int RemoveAccessible(char[,] grid, int width, int height)
    {
        int totalRemoved = 0;

        int[,] neighborCount = InitializeNeighborCounts(grid, width, height);
        Queue<V2> queue = EnqueueAccessible(grid, neighborCount, width, height);

        while (queue.Count > 0)
        {
            V2 roll = queue.Dequeue();
            if (grid[roll.Y, roll.X] != '@') continue;

            grid[roll.Y, roll.X] = '.';
            totalRemoved++;

            foreach (V2 dir in DIRS)
                UpdateNeighbor(grid, neighborCount, queue, width, height, roll, dir);
        }

        return totalRemoved;
    }
    
    private static int[,] InitializeNeighborCounts(char[,] grid, int width, int height)
    {
        int[,] counts = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y, x] == '@')
                    counts[y, x] = CountNeighbors(grid, x, y, width, height);
            }
        }

        return counts;
    }
    
    private static Queue<V2> EnqueueAccessible(char[,] grid, int[,] neighborCount, int width, int height)
    {
        Queue<V2> queue = new Queue<V2>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y, x] == '@' && neighborCount[y, x] < 4)
                    queue.Enqueue(new V2(x, y));
            }
        }
        return queue;
    }
    
    private static void UpdateNeighbor(char[,] grid, int[,] neighborCount, Queue<V2> queue,
        int width, int height, V2 roll, V2 dir)
    {
        int nx = roll.X + dir.X;
        int ny = roll.Y + dir.Y;

        if (nx < 0 || nx >= width || ny < 0 || ny >= height) return;
        if (grid[ny, nx] != '@') return;

        neighborCount[ny, nx]--;
        if (neighborCount[ny, nx] < 4)
            queue.Enqueue(new V2(nx, ny));
    }
    
    private static int CountNeighbors(char[,] grid, int x, int y, int width, int height)
    {
        int neighbors = 0;
        foreach (V2 dir in DIRS)
        {
            int nx = x + dir.X;
            int ny = y + dir.Y;
            if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;
            if (grid[ny, nx] == '@') neighbors++;
        }
        return neighbors;
    }
}
