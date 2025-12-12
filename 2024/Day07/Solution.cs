namespace AdventOfCode.Y2024.Day07;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Bridge Repair")]
class Solution : Solver {

    public object PartOne(string input) => Filter(input, AddAndMultiply).Sum();
    public object PartTwo(string input) => Filter(input, ConcatAddAndMultiply).Sum();
    
    private static IEnumerable<long> Filter(string input, Func<long, long, long[], bool> check)
    {
        return input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                long[] parts = Regex.Matches(line, @"\d+")
                    .Select(m => long.Parse(m.Value))
                    .ToArray();
                long target = parts[0];
                long[] nums = parts[1..];
                return (target, nums);
            })
            .Where(t => check(t.target, t.nums[0], t.nums[1..]))
            .Select(t => t.target);
    }
    
    private static bool AddAndMultiply(long target, long total, long[] numbers) =>
        numbers.Length == 0
            ? target == total
            : AddAndMultiply(target, total * numbers[0], numbers[1..]) ||
              AddAndMultiply(target, total + numbers[0], numbers[1..]);
    
    private static bool ConcatAddAndMultiply(long target, long total, long[] numbers) =>
        numbers.Length == 0
            ? target == total
            : numbers.Length > 0 && (
                (long.Parse($"{total}{numbers[0]}") <= target && ConcatAddAndMultiply(target, long.Parse($"{total}{numbers[0]}"), numbers[1..])) ||
                ConcatAddAndMultiply(target, total * numbers[0], numbers[1..]) ||
                ConcatAddAndMultiply(target, total + numbers[0], numbers[1..])
            );
}