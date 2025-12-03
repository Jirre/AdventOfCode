using System;
using System.Numerics;

namespace AdventOfCode.Y2025.Day02;

using System.Collections.Generic;
using System.Linq;

[ProblemName("Gift Shop")]
class Solution : Solver {

    private readonly record struct Range(BigInteger Start, BigInteger End);

    #region --- Part 1 ---
    
    public object PartOne(string input)
    {
        List<Range> parsedRanges = ParseRanges(input);
        if (parsedRanges.Count == 0)
            return BigInteger.Zero;

        List<Range> mergedRanges = MergeRanges(parsedRanges);
        BigInteger maxEnd = mergedRanges[^1].End;

        HashSet<BigInteger> invalidIds = GenerateSimpleInvalidIds(maxEnd);

        return SumIds(invalidIds, mergedRanges);
    }
    
    /// <summary>
    /// Number is invalid if it equals X concatenated with X (single repeat).
    /// </summary>
    private HashSet<BigInteger> GenerateSimpleInvalidIds(BigInteger maxEnd)
    {
        HashSet<BigInteger> invalid = new HashSet<BigInteger>();

        for (int digitCount = 1; digitCount <= 9; digitCount++)
        {
            BigInteger repetitionFactor = BigInteger.Pow(10, digitCount) + 1;
            BigInteger minPattern = BigInteger.Pow(10, digitCount - 1);
            BigInteger maxPattern = BigInteger.Pow(10, digitCount) - 1;

            for (BigInteger pattern = minPattern; pattern <= maxPattern; pattern++)
            {
                BigInteger id = pattern * repetitionFactor;
                if (id > maxEnd)
                    break;

                invalid.Add(id);
            }
        }

        return invalid;
    }
    
    #endregion
    
    #region --- Part 2 ---
    
    public object PartTwo(string input)
    {
        List<Range> parsedRanges = ParseRanges(input);
        if (parsedRanges.Count == 0)
            return BigInteger.Zero;

        List<Range> mergedRanges = MergeRanges(parsedRanges);
        BigInteger maxEnd = mergedRanges[^1].End;

        HashSet<BigInteger> invalidIds = GenerateInvalidIds(maxEnd);

        return SumIds(invalidIds, mergedRanges);
    }

    /// <summary>
    /// invalid if made of a repeated digit-sequence (>= 2 repeats).
    /// </summary>
    /// <example>
    ///     123123123, 121212, 111111
    /// </example>
    private HashSet<BigInteger> GenerateInvalidIds(BigInteger maxEnd)
    {
        HashSet<BigInteger> invalidIds = new HashSet<BigInteger>();
        int maxDigits = maxEnd.ToString().Length;
        int maxPatternLength = maxDigits / 2;

        for (int patternLength = 1; patternLength <= maxPatternLength; patternLength++)
        {
            BigInteger minPattern = BigInteger.Pow(10, patternLength - 1);
            BigInteger maxPattern = BigInteger.Pow(10, patternLength) - 1;

            foreach (BigInteger pattern in EnumerateRange(minPattern, maxPattern))
            {
                foreach (BigInteger repeated in EnumerateInvalidIds(pattern, patternLength, maxDigits, maxEnd))
                {
                    invalidIds.Add(repeated);
                }
            }
        }

        return invalidIds;
    }
    
    private IEnumerable<BigInteger> EnumerateInvalidIds(
        BigInteger pattern,
        int patternLength,
        int maxDigits,
        BigInteger maxEnd)
    {
        BigInteger pow = BigInteger.Pow(10, patternLength);
        BigInteger current = pattern;

        while (true)
        {
            current = current * pow + pattern;

            if (current.ToString().Length > maxDigits ||
                current > maxEnd)
                yield break;

            yield return current;
        }
    }
    
    #endregion
    
    /// <summary>
    /// Parses "10-20,30-40" into a list of ranges.
    /// </summary>
    private static List<Range> ParseRanges(string input)
    {
        List<Range> result = [];

        foreach (string part in input.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            string trimmed = part.Trim();
            string[] pieces = trimmed.Split('-');
            if (pieces.Length != 2)
                continue;

            if (!BigInteger.TryParse(pieces[0], out BigInteger start)) continue;
            if (!BigInteger.TryParse(pieces[1], out BigInteger end)) continue;

            if (start <= end)
                result.Add(new Range(start, end));
        }

        return result;
    }
    
    /// <summary>
    /// Merge overlapping or adjacent ranges into a clean list.
    /// </summary>
    private static List<Range> MergeRanges(List<Range> ranges)
    {
        if (ranges.Count <= 1)
            return ranges;

        List<Range> sorted = ranges.OrderBy(r => r.Start).ToList();
        List<Range> merged = new();

        Range current = sorted[0];

        for (int i = 1; i < sorted.Count; i++)
        {
            Range next = sorted[i];

            if (next.Start <= current.End + 1)
            {
                current = current with { End = BigInteger.Max(current.End, next.End) };
            }
            else
            {
                merged.Add(current);
                current = next;
            }
        }

        merged.Add(current);
        return merged;
    }
    
    /// <summary>
    /// Returns true if the given number is within any merged range
    /// </summary>
    private static bool IsInRanges(BigInteger value, List<Range> ranges)
    {
        int low = 0;
        int high = ranges.Count - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            Range r = ranges[mid];

            if (value < r.Start) high = mid - 1;
            else if (value > r.End) low = mid + 1;
            else return true;
        }

        return false;
    }
    
    /// <summary>
    /// Sums all invalid IDs that fall within the ranges
    /// </summary>
    private static BigInteger SumIds(HashSet<BigInteger> invalidIds, List<Range> mergedRanges) =>  
        invalidIds
            .AsParallel()
            .Where(id => IsInRanges(id, mergedRanges))
            .Aggregate(BigInteger.Zero, (total, id) => total + id);
    
    private static IEnumerable<BigInteger> EnumerateRange(BigInteger start, BigInteger end)
    {
        for (BigInteger i = start; i <= end; i++)
            yield return i;
    }
}