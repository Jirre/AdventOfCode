namespace AdventOfCode.Y2024.Day02;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Red-Nosed Reports")]
class Solution : Solver {

    public object PartOne(string input) =>
        ParseLine(input).Count(IsSafe);

    public object PartTwo(string input) =>
        ParseLine(input).Count(levels =>
            IsSafe(levels) || PurgeTolerance(levels).Any(IsSafe)
        );
    
    private static IEnumerable<int[]> ParseLine(string input)
    {
        foreach (string line in input.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            yield return line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
        }
    }
    
    private static bool IsSafe(int[] levels)
    {
        if (levels.Length < 2)
            return true;

        bool inc = true;
        bool dec = true;

        for (int i = 0; i < levels.Length - 1; i++)
        {
            int diff = levels[i + 1] - levels[i];

            inc &= (diff >= 1 && diff <= 3);
            dec &= (diff <= -1 && diff >= -3);

            if (!inc && !dec)
                return false;
        }

        return true;
    }
    
    private static IEnumerable<int[]> PurgeTolerance(int[] levels)
    {
        for (int remove = 0; remove < levels.Length; remove++)
        {
            int[] arr = new int[levels.Length - 1];
            int idx = 0;

            for (int i = 0; i < levels.Length; i++)
            {
                if (i == remove) continue;
                arr[idx++] = levels[i];
            }

            yield return arr;
        }
    }
}