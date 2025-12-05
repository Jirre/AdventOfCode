namespace AdventOfCode.Y2025.Day05;

using System;
using System.Collections.Generic;

[ProblemName("Cafeteria")]
class Solution : Solver {

    public object PartOne(string input)
    {
        (IReadOnlyList<Range> ranges, List<long> ids) = ParseInput(input);

        int fresh = 0;
        foreach (long id in ids)
        {
            if (IsFresh(id, ranges))
                fresh++;
        }
        return fresh;
    }

    public object PartTwo(string input)
    {
        (IReadOnlyList<Range> ranges, _) = ParseInput(input);

        long total = 0;
        foreach (Range r in ranges)
            total += (r.End - r.Start + 1);

        return total;
    }
    
    private static (IReadOnlyList<Range> ranges, List<long> ids) ParseInput(string input)
    {
        string[] sections = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        // -------- RANGES -------- //
        string[] lines = sections[0].Split('\n', StringSplitOptions.RemoveEmptyEntries);

        List<Range> all = new List<Range>(lines.Length);

        foreach (string line in lines)
        {
            int dash = line.IndexOf('-');
            long start = long.Parse(line.AsSpan(0, dash));
            long end   = long.Parse(line.AsSpan(dash + 1));

            all.Add(new Range(start, end));
        }

        all.Sort((a, b) => a.Start.CompareTo(b.Start));

        List<Range> merged = new List<Range>(all.Count);

        foreach (Range r in all)
        {
            if (merged.Count == 0 || r.Start > merged[^1].End + 1)
            {
                merged.Add(r);
            }
            else
            {
                Range last = merged[^1];
                merged[^1] = new Range(last.Start, Math.Max(last.End, r.End));
            }
        }

        // -------- IDS -------- //
        string[] idLines = sections[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
        List<long> ids = new List<long>(idLines.Length);

        foreach (string s in idLines)
            ids.Add(long.Parse(s));

        return (merged, ids);
    }

    private static bool IsFresh(long id, IReadOnlyList<Range> ranges)
    {
        int l = 0;
        int r = ranges.Count - 1;

        while (l <= r)
        {
            int mid = (l + r) >> 1;
            Range m = ranges[mid];

            if (id < m.Start) r = mid - 1;
            else if (id > m.End) l = mid + 1;
            else return true;
        }

        return false;
    }
    
    private readonly struct Range(long pStart, long pEnd)
    {
        public readonly long Start = pStart;
        public readonly long End = pEnd;
    }
}