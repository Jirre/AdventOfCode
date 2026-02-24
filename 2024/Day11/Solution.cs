namespace AdventOfCode.Y2024.Day11;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Plutonian Pebbles")]
class Solution : Solver
{
    private readonly Dictionary<(long value, int steps), long> _cache = new();
    
    public object PartOne(string input) => Solve(input, 25);
    public object PartTwo(string input) => Solve(input, 75);
    
    private long Solve(string input, int steps)
    {
        _cache.Clear();
        return input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .Sum(n => Step(n, steps));
    }
    
    private long Step(long value, int steps)
    {
        if (steps == 0) return 1;
        if (_cache.TryGetValue((value, steps), out long cached)) return cached;

        long result = value switch
        {
            0 => Step(1, steps - 1),
            _ when IsEven(value, out int length) => 
                Divide(value, length, steps),
            _ => Step(value * 2024, steps - 1)
        };

        return _cache[(value, steps)] = result;
    }
    
    private static bool IsEven(long n, out int length)
    {
        length = (int)Math.Floor(Math.Log10(n)) + 1;
        return length % 2 == 0;
    }
    
    private long Divide(long n, int length, int steps)
    {
        long divisor = (long)Math.Pow(10, length / 2);
        long left = n / divisor;
        long right = n % divisor;

        return Step(left, steps - 1) + Step(right, steps - 1);
    }
}
