using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Numerics;

namespace AdventOfCode.Y2024.Day13;

[ProblemName("Claw Contraption")]
class Solution : Solver {

    private record Context(Vector2Long AMovement, Vector2Long BMovement, Vector2Long Target);
    
    public object PartOne(string input) => Parse(input).Sum(GetCost);
    public object PartTwo(string input) => Parse(input, offset: 10000000000000).Sum(GetCost);
    
    /// <summary>
    /// Parses the input blocks into Machine records.
    /// </summary>
    private IEnumerable<Context> Parse(string input, long offset = 0) {
        string[] blocks = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        foreach (string block in blocks) {
            long[] coordinates = Regex.Matches(block, @"\d+")
                .Select(m => long.Parse(m.Value))
                .ToArray();
            
            Vector2Long buttonA = new(coordinates[0], coordinates[1]);
            Vector2Long buttonB = new(coordinates[2], coordinates[3]);
            Vector2Long target  = new(coordinates[4] + offset, coordinates[5] + offset);

            yield return new Context(buttonA, buttonB, target);
        }
    }
    
    /// <summary>
    /// Calculates the minimum number of presses required to reach the target
    /// (A is multiplied by 3 as per the puzzle description)
    /// </summary>
    private static long GetCost(Context context)
    {
        Vector2Long a = context.AMovement;
        Vector2Long b = context.BMovement;
        Vector2Long target = context.Target;
        
        long mainDet = Determinant(a, b);
        // Check if there is a solution
        if (mainDet == 0) 
            return 0; 
        
        long aPresses = Determinant(target, b) / mainDet;
        long bPresses = Determinant(a, target) / mainDet;
        
        // Validate solution due to rounding errors
        bool validX = a.x * aPresses + b.x * bPresses == target.x;
        bool validY = a.y * aPresses + b.y * bPresses == target.y;
        
        if (aPresses >= 0 && bPresses >= 0 && validX && validY) {
            return (aPresses * 3) + bPresses;
        }

        return 0;
    }
    
    private static long Determinant(Vector2Long a, Vector2Long b) => 
        a.x * b.y - a.y * b.x;
}