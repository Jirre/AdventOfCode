namespace AdventOfCode.Y2025.Day03;

using System;

[ProblemName("Lobby")]
class Solution : Solver {
    
    public object PartOne(string input) =>
        Solve(input, 2);

    public object PartTwo(string input) =>
        Solve(input, 12);
    
    /// <summary>
    /// Computes the total maximum value for the given input by selecting the highest <paramref name="digitCount"/>-digit number that can be formed from each line while preserving the left-to-right order of digits.
    /// </summary>
    /// <param name="input">Multi-line input where each line is a bank of digit characters.</param>
    /// <param name="digitCount">Number of digits to select from each line to form the final value.</param>
    /// <returns>The sum of the maximum values computed for each input line.</returns>
    private static long Solve(string input, int digitCount)
    {
        long total = 0;
        
        long[] powers = new long[digitCount];
        for (int i = 0; i < digitCount; i++)
            powers[i] = FastPow10(digitCount - (i + 1));
        
        foreach (ReadOnlySpan<char> line in input.AsSpan().EnumerateLines())
        {
            long result = 0;
            int startIndex = 0;

            for (int i = 0; i < digitCount; i++)
            {
                int remaining = digitCount - (i + 1);
                int lastIndex = line.Length - 1 - remaining;
                int searchLength = lastIndex - startIndex + 1;

                ReadOnlySpan<char> searchSegment = line.Slice(startIndex, searchLength);

                FindMaxInSpan(searchSegment, startIndex, out int digit, out int index);

                result += digit * powers[i];
                startIndex = index + 1;
            }

            total += result;
        }

        return total;
    }
    
    /// <summary>
    /// Scans the provided <paramref name="span"/> for the maximum single digit (0–9) and returns that digit along with its global index (computed as the local index within <paramref name="span"/> plus the supplied <paramref name="offset"/>).
    /// </summary>
    /// <param name="span">The span of digit characters to search</param>
    /// <param name="offset">Offset to add to the local index to produce the returned global index</param>
    /// <param name="value">Outputs the maximum digit found</param>
    /// <param name="index">Outputs the global index of the found digit</param>
    private static void FindMaxInSpan(ReadOnlySpan<char> span, int offset, out int value, out int index)
    {
        value = -1;
        index = 0;

        for (int i = 0; i < span.Length; i++)
        {
            int d = span[i] - '0';

            if (d <= value) 
                continue;
            
            value = d;
            index = i + offset;

            if (d == 9)
                break; // early exit
        }
    }
    
    /// <summary>
    /// Computes 10 raised to the power of <paramref name="exp"/> using a simple multiplication loop.
    /// This avoids the overhead and floating-point behavior of <c>Math.Pow</c> and is faster for small integer exponents.
    /// </summary>
    /// <param name="exp">The non-negative exponent.</param>
    /// <returns>A <see cref="long"/> equal to 10^exp.</returns>
    private static long FastPow10(int exp)
    {
        long v = 1;
        while (exp-- > 0) v *= 10;
        return v;
    }
}