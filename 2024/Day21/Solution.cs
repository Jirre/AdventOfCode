using System.Collections.Concurrent;
using Shared.Numerics;

namespace AdventOfCode.Y2024.Day21;

using System;
using System.Collections.Generic;
using Cache = ConcurrentDictionary<(char currentKey, char nextKey, int depth), long>;

[ProblemName("Keypad Conundrum")]
class Solution : Solver {
    
    private class Keypad
    {
        private readonly Dictionary<Vector2Int, char> _keys;
        private readonly Dictionary<char, Vector2Int> _positions;

        public Keypad(string layout)
        {
            _keys = new Dictionary<Vector2Int, char>();
            _positions = new Dictionary<char, Vector2Int>();

            string[] lines = layout.Split('\n');

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    Vector2Int position = new Vector2Int(x, -y);
                    char key = lines[y][x];

                    _keys[position] = key;
                    _positions[key] = position;
                }
            }
        }

        public Vector2Int GetPosition(char key) =>
            _positions[key];

        public bool IsEmpty(Vector2Int position) =>
            !_keys.TryGetValue(position, out char key) || key == ' ';
    }

    public object PartOne(string input) => Solve(input, 2);

    public object PartTwo(string input) => Solve(input, 25);
    
    private long Solve(string input, int robotCount)
    {
        Keypad numericKeypad = new(
            "789\n" +
            "456\n" +
            "123\n" +
            " 0A");

        Keypad directionalKeypad = new(
            " ^A\n" +
            "<v>");

        Keypad[] keypads = new Keypad[robotCount + 1];
        keypads[0] = numericKeypad;
        Array.Fill(keypads, directionalKeypad, 1, robotCount);

        Cache cache = new();
        long total = 0;

        foreach (string line in input.Split('\n'))
        {
            if (line.Length == 0)
                continue;

            int value = int.Parse(line[..^1]);
            total += value * EncodeKeys(line, keypads, 0, cache);
        }

        return total;
    }

    /// <summary>
    /// Computes the total encoded cost for transitioning between a sequence of keys on a keypad,
    /// using recursive operations and caching for optimization.
    /// </summary>
    private long EncodeKeys(
        string keys,
        Keypad[] keypads,
        int keypadIndex,
        Cache cache)
    {
        if (keypadIndex >= keypads.Length)
            return keys.Length;

        int depth = keypads.Length - keypadIndex;
        long result = 0;
        char current = 'A';

        foreach (char next in keys)
        {
            char c = current;
            result += cache.GetOrAdd(
                (current, next, depth),
                _ => CalcKeyCost(c, next, keypads, keypadIndex, cache));
            current = next;
        }

        return result;
    }

    /// <summary>
    /// Calculates the minimum cost to move between two keys on a keypad while considering
    /// possible intermediate steps and constraints.
    /// </summary>
    private long CalcKeyCost(
        char currentKey,
        char nextKey,
        Keypad[] keypads,
        int keypadIndex,
        Cache cache)
    {
        Keypad keypad = keypads[keypadIndex];

        Vector2Int current = keypad.GetPosition(currentKey);
        Vector2Int next = keypad.GetPosition(nextKey);

        int dx = next.x - current.x;
        int dy = next.y - current.y;

        string horizontal = CreateMoves(dx, '<', '>');
        string vertical = CreateMoves(dy, 'v', '^');

        long best = long.MaxValue;

        // Try vertical -> horizontal.
        if (!keypad.IsEmpty(new Vector2Int(current.x, next.y)))
        {
            best = Math.Min(
                best,
                EncodeKeys(
                    vertical + horizontal + "A",
                    keypads,
                    keypadIndex + 1,
                    cache));
        }

        // Try horizontal -> vertical.
        if (!keypad.IsEmpty(new Vector2Int(next.x, current.y)))
        {
            best = Math.Min(
                best,
                EncodeKeys(
                    horizontal + vertical + "A",
                    keypads,
                    keypadIndex + 1,
                    cache));
        }

        return best;
    }

    /// <summary>
    /// Creates a sequence of moves represented as a string based on the provided delta and direction characters.
    /// </summary>
    /// <returns>A string representing the sequence of moves. Returns an empty string if <paramref name="delta"/> is 0.</returns>
    private static string CreateMoves(
        int delta,
        char negativeDirection,
        char positiveDirection)
    {
        if (delta == 0)
            return string.Empty;

        return new string(
            delta < 0 ? negativeDirection : positiveDirection,
            Math.Abs(delta));
    }
}