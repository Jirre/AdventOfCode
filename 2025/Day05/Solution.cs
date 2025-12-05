namespace AdventOfCode.Y2025.Day05;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Cafeteria")]
class Solution : Solver {

    public object PartOne(string input) {
        (List<(long start, long end)> ranges, List<long> ids) = ParseInput(input);
        return ids.Count(id => IsFresh(id, ranges));
    }

    public object PartTwo(string input)
    {
        (List<(long start, long end)> ranges, _) = ParseInput(input);
        return ranges.Sum(r => r.end - r.start + 1);
    }
    
    private static (List<(long start, long end)> ranges, List<long> ids) ParseInput(string input)
    {
        string[] sections = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        // --- Ranges --- //
        string[] rangeLines = sections[0]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);

        List<(long start, long end)> allRanges = rangeLines
            .Select(line =>
            {
                string[] parts = line.Split('-');
                return (start: long.Parse(parts[0]), end: long.Parse(parts[1]));
            })
            .OrderBy(r => r.start)
            .ToList();
        
        List<(long start, long end)> ranges = new List<(long start, long end)>();
        foreach ((long start, long end) r in allRanges)
        {
            if (ranges.Count == 0 || r.start > ranges[^1].end + 1)
                ranges.Add(r);
            else
                ranges[^1] = (ranges[^1].start, Math.Max(ranges[^1].end, r.end));
        }
        
        // --- IDs --- //
        string[] idLines = sections[1]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);

        List<long> ids = idLines
            .Select(long.Parse)
            .ToList();

        return (ranges, ids);
    }

    private static bool IsFresh(long id, List<(long start, long end)> ranges)
    {
        int l = 0;
        int r = ranges.Count - 1;

        while (l <= r)
        {
            int mid = l + ((r - l) >> 1);
            (long start, long end) = ranges[mid];

            if (id < start) r = mid - 1;
            else if (id > end) l = mid + 1;
            else return true;
        }

        return false;
    }
}