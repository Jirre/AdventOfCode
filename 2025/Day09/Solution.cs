namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Movie Theater")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var reds = ParsePoints(input);
        long maxArea = 0;

        for (int i = 0; i < reds.Count; i++)
        {
            for (int j = i + 1; j < reds.Count; j++)
            {
                long width = Math.Abs(reds[j].x - reds[i].x) + 1;
                long height = Math.Abs(reds[j].y - reds[i].y) + 1;
                maxArea = Math.Max(maxArea, width * height);
            }
        }

        return maxArea;
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private static List<(long x, long y)> ParsePoints(string input)
    {
        return input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                var parts = line.Split(',');
                return (x: long.Parse(parts[0]), y: long.Parse(parts[1]));
            }).ToList();
    }
}
