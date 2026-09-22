using System.Linq;

namespace AdventOfCode.Y2024.Day22;

[ProblemName("Monkey Market")]
class Solution : Solver
{
    private const int ITERATIONS = 2000;
    private const int SEQUENCE_COUNT = 19 * 19 * 19 * 19;
    private const int SEQUENCE_BASE = 19;
    private const int SEQUENCE_OFFSET = 9;
    private const int SEQUENCE_PREFIX_SIZE = 19 * 19 * 19;

    public object PartOne(string input)
    {
        long total = 0;

        foreach (int seed in ParseSeeds(input))
        {
            int value = seed;

            for (int i = 0; i < ITERATIONS; i++)
            {
                value = CalcValue(value);
            }

            total += value;
        }

        return total;
    }

    public object PartTwo(string input)
    {
        int[] totals = new int[SEQUENCE_COUNT];
        int[] seen = new int[SEQUENCE_COUNT];
        int seedIndex = 0;

        foreach (int seed in ParseSeeds(input))
        {
            seedIndex++;
            CalcSequence(seed, seedIndex, seen, totals);
        }

        return totals.Max();
    }
    
    private static int[] ParseSeeds(string input) =>
        [.. input.Split('\n').Select(int.Parse)];

    /// <summary>
    /// Calculates a sequence based on the provided seed and updates tracking arrays
    /// for sequences seen and their corresponding totals.
    /// </summary>
    private static void CalcSequence(
        int seed,
        int seedIndex,
        int[] seen,
        int[] totals)
    {
        int value = seed;
        int previousDigit = value % 10;
        int sequence = 0;

        for (int i = 1; i <= ITERATIONS; i++)
        {
            value = CalcValue(value);

            int digit = value % 10;
            int difference = digit - previousDigit;

            /*
             * Encode four differences as a base-19 number.
             *
             * Differences range from -9 to 9, so adding 9 maps them
             * to the range 0..18.
             */
            sequence =
                (sequence % SEQUENCE_PREFIX_SIZE) * SEQUENCE_BASE
                + difference + SEQUENCE_OFFSET;

            if (i >= 4 && seen[sequence] != seedIndex)
            {
                seen[sequence] = seedIndex;
                totals[sequence] += digit;
            }

            previousDigit = digit;
        }
    }

    /// <summary>
    /// Calculates the transformed integer based on the base input
    /// (Transformations as per the rules defined in the exercise)
    /// </summary>
    private static int CalcValue(int value)
    {
        value = Mix(value, value << 6);
        value = Mix(value, value >> 5);
        value = Mix(value, value << 11);

        return value;
    }

    private static int Mix(int value, int transformedValue)
    {
        return (value ^ transformedValue) & 0xFFFFFF;
    }
}