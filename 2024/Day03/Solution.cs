namespace AdventOfCode.Y2024.Day03;

using System.Text.RegularExpressions;

[ProblemName("Mull It Over")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, @"mul\((\d{1,3}),(\d{1,3})\)");

    public object PartTwo(string input) => Solve(input, @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");

    private static long Solve(string input, string pattern)
    {
        Regex regex = new Regex(pattern, RegexOptions.Multiline);
        bool enabled = true;
        long result = 0;

        foreach (Match m in regex.Matches(input))
        {
            string val = m.Value;

            enabled = val switch
            {
                "don't()" => false,
                "do()" => true,
                _ => enabled
            };

            if (!enabled || !val.StartsWith("mul(")) 
                continue;
            
            int a = int.Parse(m.Groups[1].Value);
            int b = int.Parse(m.Groups[2].Value);
            result += (long)a * b;
        }

        return result;
    }
}
