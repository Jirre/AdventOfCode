using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2024.Day19;

[ProblemName("Linen Layout")]
class Solution : Solver {

    public object PartOne(string input) => 
        GetMatchCounts(input).Count(count => count > 0);

    public object PartTwo(string input) => 
        GetMatchCounts(input).Sum();

    private IEnumerable<long> GetMatchCounts(string input) {
        string[] blocks = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        Dictionary<char, string[]> groups = blocks[0].Split(", ")
            .GroupBy(t => t[0])
            .ToDictionary(g => g.Key, g => g.ToArray());

        string[] patterns = blocks[1].Split("\n", StringSplitOptions.RemoveEmptyEntries);

        foreach (string pattern in patterns) {
            long[] cache = new long[pattern.Length + 1];
            Array.Fill(cache, -1); 

            yield return Calc(pattern, 0, groups, cache);
        }
    }

    private long Calc(string pattern, int start, Dictionary<char, string[]> groups, long[] cache) {
        if (start == pattern.Length) return 1;
        if (cache[start] != -1) return cache[start];

        long totalWays = 0;
        char firstChar = pattern[start];

        if (groups.TryGetValue(firstChar, out string[] relevantTowels)) {
            ReadOnlySpan<char> remaining = pattern.AsSpan(start);

            foreach (string towel in relevantTowels) {
                if (remaining.StartsWith(towel)) {
                    totalWays += Calc(pattern, start + towel.Length, groups, cache);
                }
            }
        }

        cache[start] = totalWays;
        return totalWays;
    }
}