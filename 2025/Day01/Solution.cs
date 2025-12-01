using System;

namespace AdventOfCode.Y2025.Day01;

[ProblemName("Secret Entrance")]
class Solution : Solver {
    
    private const int MODULO = 100;
    private const int START_POS = 50;

    public object PartOne(string input) {
        int pos = START_POS;
        int count = 0;

        foreach (ReadOnlySpan<char> line in input.AsSpan().EnumerateLines())
        {
            if (!TryParseLine(line, out int sign, out int delta))
                continue;

            pos = Wrap(pos + sign * delta);

            if (pos == 0)
                count++;
        }

        return count;
    }

    public object PartTwo(string input) {
        int pos = START_POS;
        int collisions = 0;

        foreach (var line in input.AsSpan().EnumerateLines())
        {
            if (!TryParseLine(line, out int sign, out int delta))
                continue;

            // Solve for first hit of 0
            int target = (MODULO - (pos * sign % MODULO)) % MODULO;

            if (target == 0)
                target = MODULO;

            if (target <= delta)
                collisions += 1 + (delta - target) / MODULO;

            // Update position after a cycle
            pos = Wrap(pos + sign * delta);
        }

        return collisions;
    }


    /// <summary>
    /// Attempts to parse a line of input.
    /// </summary>
    /// <param name="line">The input line as a <see cref="ReadOnlySpan{Char}"/> to parse.</param>
    /// <param name="sign"> The direction sign. (R = 1, L = -1) </param>
    /// <param name="delta"> The delta value parsed from the input line. </param>
    /// <returns> Was the line parsed successfully, and is the data valid </returns>
    private static bool TryParseLine(ReadOnlySpan<char> line, out int sign, out int delta) {
        sign = 0;
        delta = 0;

        if (line.IsEmpty)
            return false;

        char dir = line[0];
        sign = dir switch
        {
            'R' => 1,
            'L' => -1,
            _ => 0
        };

        return sign != 0 && int.TryParse(line[1..], out delta);
    }

    /// <summary>
    /// Wraps a position around the modulo
    /// </summary>
    private static int Wrap(int pValue)
    {
        pValue %= MODULO;
        return pValue < 0 ? pValue + MODULO : pValue;
    }
}
