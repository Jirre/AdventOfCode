namespace AdventOfCode.Y2025.Day06;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Trash Compactor")]
class Solution : Solver {

    public object PartOne(string input)
    {
        List<string> lines = input.Split('\n').Select(l => l.TrimEnd('\r')).ToList();
        List<List<string>> columns = ParseColumns(lines);

        long total = 0;

        foreach (List<string> group in columns)
        {
            long result = Evaluate(group);
            total += result;
        }

        return total;
    }

    private static long Evaluate(List<string> columns)
    {
        int height = columns[0].Length;
        List<string> rows = new List<string>();

        for (int r = 0; r < height; r++)
        {
            char[] chars = new char[columns.Count];
            for (int c = 0; c < columns.Count; c++)
            {
                chars[c] = columns[c][r];
            }
            rows.Add(new string(chars));
        }
        
        List<long> numbers = new List<long>();
        for (int i = 0; i < rows.Count - 1; i++)
        {
            string trimmed = rows[i].Trim();
            if (trimmed.Length == 0)
                continue;

            numbers.Add(long.Parse(trimmed));
        }

        // Get the operator
        string lastRow = rows[^1].Trim();
        
        return lastRow[0] switch
        {
            '+' => numbers.Sum(),
            '*' => numbers.Aggregate(1L, (acc, n) => acc * n),
            _ => throw new Exception("Unknown operator: " + lastRow[0])
        };
    }

    public object PartTwo(string input)
    {
        List<string> lines = input.Split('\n').Select(l => l.TrimEnd('\r')).ToList();
        List<List<string>> columns = ParseColumns(lines);

        long total = 0;

        foreach (List<string> problem in columns)
        {
            total += EvaluateRTL(problem);
        }

        return total;
    }

    // ReSharper disable once InconsistentNaming
    private static long EvaluateRTL(List<string> columns)
    {
        int height = columns[0].Length;
        char op = columns.Select(col => col[height - 1]).First(c => c != ' ');
        
        List<long> numberList = new List<long>();

        for (int i = columns.Count - 1; i >= 0; i--)
        {
            List<char> digits = new List<char>();
            
            for (int row = 0; row < height - 1; row++)
            {
                char ch = columns[i][row];
                if (char.IsDigit(ch))
                    digits.Add(ch);
            }

            if (digits.Count <= 0) continue;
            
            long num = long.Parse(new string(digits.ToArray()));
            numberList.Add(num);
        }

        return op switch
        {
            '+' => numberList.Sum(),
            '*' => numberList.Aggregate(1L, (acc, n) => acc * n),
            _ => throw new Exception("Unknown operator: " + op)
        };
    }
    
    private static List<List<string>> ParseColumns(List<string> lines)
    {
        int width = lines.Max(l => l.Length);
        List<string> cols =  Enumerable.Range(0, width)
            .Select(c => new string(lines.Select(line => c < line.Length ? line[c] : ' ').ToArray()))
            .ToList();
        
        List<List<string>> results = new List<List<string>>();
        List<string> current = new List<string>();

        foreach (string col in cols)
        {
            bool isBlank = col.All(ch => ch == ' '); 

            if (isBlank)
            {
                // End of a problem group
                if (current.Count <= 0) continue;
                results.Add(new List<string>(current));
                current.Clear();
            }
            else
            {
                // Add non-blank column to results
                current.Add(col);
            }
        }

        if (current.Count > 0)
            results.Add(current);

        return results;
    }
}