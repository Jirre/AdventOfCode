namespace AdventOfCode.Y2024.Day17;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Chronospatial Computer")]
class Solution : Solver {
    
    private enum EOpCode 
    {
        Adv, 
        Bxl, 
        Bst, 
        Jnz, 
        Bxc, 
        Out, 
        Bdv, 
        Cdv
    }
    
    public object PartOne(string input) 
    {
        (long[] regs, List<long> program) = Parse(input);
        return string.Join(",", Solve(regs[0], regs[1], regs[2], program));
    }

    public object PartTwo(string input) 
    {
        (_, List<long> program) = Parse(input);
        return SolveInverse(program, program.Count - 1, 0);
    }
    
    private long SolveInverse(List<long> target, int index, long currentA) {
        if (index < 0) return currentA;

        for (int bits = 0; bits < 8; bits++) {
            long testA = (currentA << 3) | (uint)bits;
            var output = Solve(testA, 0, 0, target);

            // Verify if this candidate produces the expected suffix
            if (output.Count <= 0 || output[0] != target[index]) continue;
            
            long result = SolveInverse(target, index - 1, testA);
            if (result != -1) return result;
        }
        return -1;
    }
    
    private static List<long> Solve(long a, long b, long c, List<long> program) {
        List<long> output = [];
        int ip = 0;

        while (ip < program.Count) {
            long opcode = program[ip];
            long operand = program[ip + 1];

            // Resolve Combo Operand
            long combo = operand switch {
                <= 3 => operand,
                4 => a,
                5 => b,
                6 => c,
                _ => 0
            };

            switch ((EOpCode)opcode) {
                case EOpCode.Adv: a >>= (int)combo; break;
                case EOpCode.Bxl: b ^= operand; break;
                case EOpCode.Bst: b = combo & 7; break;
                case EOpCode.Jnz: 
                    if (a != 0) { ip = (int)operand; continue; } 
                    break;
                case EOpCode.Bxc: b ^= c; break;
                case EOpCode.Out: output.Add(combo & 7); break;
                case EOpCode.Bdv: b = a >> (int)combo; break;
                case EOpCode.Cdv: c = a >> (int)combo; break;
            }
            ip += 2;
        }
        return output;
    }
    
    private static (long[] regs, List<long> program) Parse(string input) {
        List<long>[] blocks = input.Split("\n\n")
            .Select(b => 
                Regex.Matches(b, @"\d+").
                    Select(m => long.Parse(m.Value)).
                    ToList())
            .ToArray();
        return (blocks[0].ToArray(), blocks[1]);
    }
}