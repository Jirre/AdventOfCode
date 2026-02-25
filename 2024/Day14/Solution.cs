namespace AdventOfCode.Y2024.Day14;

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shared.Numerics;

[ProblemName("Restroom Redoubt")]
class Solution : Solver {
    private const int WIDTH = 101;
    private const int HEIGHT = 103;

    private class Entity(Vector2Int pos, Vector2Int velocity) {
        public Vector2Int Pos { get; private set; } = pos;
        private Vector2Int Velocity { get; } = velocity;
        
        /// <summary>
        /// Advances the entity one step.
        /// </summary>
        public void Step() {
            Pos = (Pos + Velocity + new Vector2Int(WIDTH, HEIGHT * 10)) % new Vector2Int(WIDTH, HEIGHT);
        }

        /// <summary>
        /// Returns a vector representing the quadrant ranging from (-1,-1) to (1,1).
        /// </summary>
        public Vector2Int GetQuadrant() =>
            new Vector2Int(Math.Sign(Pos.x - WIDTH / 2), Math.Sign(Pos.y - HEIGHT / 2));
    }

    public object PartOne(string input) {
        Entity[] entities = Parse(input);
        
        for (int i = 0; i < 100; i++) {
            foreach (Entity entity in entities) entity.Step();
        }

        return entities
            .Select(e => e.GetQuadrant())
            .Where(q => q.x != 0 && q.y != 0)
            .GroupBy(q => q)
            .Aggregate(1, (acc, group) => acc * group.Count());
    }

    public object PartTwo(string input) {
        Entity[] entities = Parse(input);
        int seconds = 0;

        // Search for a horizontal alignment which indicates the formation of the tree
        while (true) {
            seconds++;
            bool[,] grid = new bool[HEIGHT, WIDTH];
            foreach (Entity entity in entities)
            {
                entity.Step();
                grid[entity.Pos.y, entity.Pos.x] = true;
            }

            StringBuilder sb = new();
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    sb.Append(grid[y, x] ? '#' : ' ');
                }
                sb.AppendLine();
            }
            if (sb.ToString().Contains("#################")) {
                return seconds;
            }
        }
    }

    private static Entity[] Parse(string input) {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Entity[] entities = new Entity[lines.Length];

        for (int i = 0; i < lines.Length; i++) {
            int[] nums = Regex.Matches(lines[i], @"-?\d+")
                .Select(m => int.Parse(m.Value))
                .ToArray();

            entities[i] = new Entity(
                new Vector2Int(nums[0], nums[1]), 
                new Vector2Int(nums[2], nums[3])
            );
        }

        return entities;
    }
}