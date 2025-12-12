namespace AdventOfCode.Y2024.Day01;

using System;
using System.Collections.Generic;

[ProblemName("Historian Hysteria")]
class Solution : Solver {

    public object PartOne(string input)
    {
        List<int> left = ParseColumn(input, 0);
        List<int> right = ParseColumn(input, 1);
        
        int total = 0;
        for (int i = 0; i < left.Count; i++)
        {
            total += Math.Abs(left[i] - right[i]);
        }

        return total;
    }

    public object PartTwo(string input)
    {
        List<int> left = ParseColumn(input, 0);
        List<int> right = ParseColumn(input, 1);
        
        Dictionary<int, int> counts = new Dictionary<int, int>();
        foreach (int value in right)
        {
            if (!counts.TryAdd(value, 1))
                counts[value]++;
        }

        int total = 0;
        foreach (int n in left)
        {
            counts.TryGetValue(n, out int w);
            total += n * w;
        }

        return total;
    }

    private static List<int> ParseColumn(string input, int column)
    {
        List<int> numbers = new List<int>();

        foreach (string line in input.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split("   ");
            numbers.Add(int.Parse(parts[column]));
        }

        numbers.Sort();
        return numbers;
    }
}
