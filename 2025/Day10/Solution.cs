namespace AdventOfCode.Y2025.Day10;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.Z3;

[ProblemName("Factory")]
class Solution : AdventOfCode.Solver
{
    private record Machine(List<int[]> Operations, string TargetPattern, List<int> TargetToggles);

    #region --- Part 1 ---

    public object PartOne(string input) =>
        ParseMachines(input)
            .Sum(m => FindMinOperations(m.Operations, m.TargetPattern));

    private static int FindMinOperations(List<int[]> operations, string targetBits)
    {
        int bitCount = targetBits.Length;
        int targetPattern = ParseToInt(targetBits, "[#]");

        int[] operationMasks = operations.Select(op => ParseToMask(op, bitCount)).ToArray();
        int maskCount = operationMasks.Length;
        int result = int.MaxValue;
        
        for (int subset = 0; subset < (1 << maskCount); subset++)
        {
            int currentPattern = operationMasks
                .Where((_, index) => ((subset >> index) & 1) != 0)
                .Aggregate(0, (acc, mask) => acc ^ mask);
            
            if (currentPattern != targetPattern) continue;
            
            int presses = BitOperations.PopCount((uint)subset);
            result = Math.Min(result, presses);
        }

        return result;
    }

    #endregion
    
    #region --- Part 2 ---
    
    public object PartTwo(string input) =>
        ParseMachines(input)
            .Sum(m => FindMinToReachTargetTotal(m.Operations, m.TargetToggles));
    
    /// <summary>
    /// Finds the minimal total number of times operations must be applied to reach the target counts exactly.
    /// Uses Z3's optimization solver to minimize the sum of operation applications.
    /// </summary>
    /// <param name="operations"> A list of integer arrays, where each array contains the indices of the lights affected. </param>
    /// <param name="targetToggles"> The desired totals for each indicator to be increased by. </param>
    private static int FindMinToReachTargetTotal(List<int[]> operations, List<int> targetToggles)
    {
        if (operations.Count == 0 || targetToggles.Count == 0)
            return 0;

        using Context ctx = new Context();
        using Optimize opt = ctx.MkOptimize();
        
        // Create a Z3 integer variable for each operation
        ArithExpr[] opVars = CreateNonNegativeIntVars(ctx, opt, operations.Count, "op");
        
        for (int i = 0; i < targetToggles.Count; i++)
        {
            // Find all operations that affect this target element
            ArithExpr[] affectingVars = operations
                .Select((op, idx) => (op, idx))
                .Where(t => t.op.Contains(i))
                .Select(t => opVars[t.idx])
                .ToArray();

            if (affectingVars.Length == 0)
            {
                if (targetToggles[i] != 0)
                    throw new InvalidOperationException("No solution: target element cannot be reached.");
                continue;
            }

            // Sum all affecting operation variables (or just the single variable if only one)
            ArithExpr sumExpr = affectingVars.Length == 1 ? 
                affectingVars[0] : 
                ctx.MkAdd(affectingVars);
            
            // Add a constraint that this sum must equal the target value
            opt.Add(ctx.MkEq(sumExpr, ctx.MkInt(targetToggles[i])));
        }
        
        ArithExpr totalPresses = ctx.MkAdd(opVars);
        opt.MkMinimize(totalPresses);

        // Return number of operations required
        return opt.Check() == Status.SATISFIABLE
            ? opVars.Sum(v => ((IntNum) opt.Model.Evaluate(v)).Int)
            : throw new InvalidOperationException("No exact solution found.");
    }
    
    /// <summary>
    /// Creates an array of integer variables representing the number of times each operation is applied.
    /// </summary>
    /// <param name="ctx">The Z3 solver context.</param>
    /// <param name="count">The number of variables to create (one per operation).</param>
    /// <param name="prefix">Prefix to use for naming each variable in the solver.</param>
    /// <returns>An array of Z3 integer expressions representing the operation counts.</returns>
    private static ArithExpr[] CreateNonNegativeIntVars(Context ctx, Optimize opt, int count, string prefix)
    {
        ArithExpr[] operationParams = new ArithExpr[count];
        for (int i = 0; i < count; i++)
        {
            operationParams[i] = ctx.MkIntConst($"{prefix}_{i}");
            opt.Add(ctx.MkGe(operationParams[i], ctx.MkInt(0)));
        }
        return operationParams;
    }
    
    #endregion
    
    private static List<Machine> ParseMachines(string input)
        => input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseMachine)
                .ToList();

    private static Machine ParseMachine(string line)
    {
        string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string pattern = parts[0][1..^1]; // Remove surrounding brackets

        List<int[]> operations = parts[1..^1]
            .Select(s => s[1..^1] 
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray())
            .ToList();

        List<int> targetTotals = parts[^1][1..^1]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse).ToList();

        return new Machine(operations, pattern, targetTotals);
    }
    
    private static int ParseToInt(string bitString, string truePattern)
    {
        int value = 0;
        Regex regex = new Regex(truePattern);

        foreach (char c in bitString)
            value = (value << 1) | (regex.IsMatch(c.ToString()) ? 1 : 0);

        return value;
    }
    
    private static int ParseToMask(int[] indices, int totalBits)
    {
        int mask = 0;
        foreach (int index in indices)
            mask |= 1 << (totalBits - index - 1);
        return mask;
    }
}
