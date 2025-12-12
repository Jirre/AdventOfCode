namespace AdventOfCode.Y2024.Day05;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Print Queue")]
class Solution : Solver
{
    private readonly record struct SortingRule(string[][] Updates, Comparer<string> Comparer);
    
    public object PartOne(string input)
    {
        SortingRule rules = ParseLine(input);

        return rules.Updates
            .Where(pages => IsSorted(pages, rules.Comparer))
            .Sum(GetMiddlePage);
    }

    public object PartTwo(string input)
    {
        SortingRule rules = ParseLine(input);

        return rules.Updates
            .Where(pages => !IsSorted(pages, rules.Comparer))
            .Select(pages => pages.OrderBy(p => p, rules.Comparer).ToArray())
            .Sum(GetMiddlePage);
    }

    private static SortingRule ParseLine(string input)
    {
        string[] parts = input.Split("\n\n");

        HashSet<string> ordering = new HashSet<string>(parts[0].Split("\n"));

        Comparer<string> comparer = Comparer<string>.Create((p1, p2) =>
            ordering.Contains(p1 + "|" + p2) ? -1 : 1);

        string[][] updates = parts[1].Split("\n")
            .Select(line => line.Split(","))
            .ToArray();

        return new SortingRule(updates, comparer);
    }
    
    private static bool IsSorted(string[] pages, Comparer<string> comparer) =>
        pages.SequenceEqual(pages.OrderBy(x => x, comparer));

    private static int GetMiddlePage(string[] pages) => int.Parse(pages[pages.Length / 2]);
}
