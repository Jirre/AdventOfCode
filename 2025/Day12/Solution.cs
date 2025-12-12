namespace AdventOfCode.Y2025.Day12;

using System;
using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("Christmas Tree Farm")]
class Solution : Solver {

    private record Shape(int W, int H, int[] Counts);
    
    public object PartOne(string input) {
        string[] cells = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        int[] areas = ParseAreas(cells[..^1]);
        Shape[] shapes = ParseShapes(cells[^1]);

        return shapes.Count(shape => Fits(shape, areas));
    }

    private static int[] ParseAreas(string[] cells) {
        int[] areas = new int[cells.Length];

        for (int i = 0; i < cells.Length; i++) {
            int area = 0;
            foreach (char ch in cells[i]) {
                if (ch == '#') area++;
            }
            areas[i] = area;
        }

        return areas;
    }

    private static Shape[] ParseShapes(string cells) {
        string[] lines = cells.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Shape[] shapes = new Shape[lines.Length];

        for (int i = 0; i < lines.Length; i++) {
            int[] numbers = Regex.Matches(lines[i], @"\d+")
                .Select(m => int.Parse(m.Value))
                .ToArray();

            int w = numbers[0];
            int h = numbers[1];
            int[] counts = numbers[2..];

            shapes[i] = new Shape(w, h, counts);
        }

        return shapes;
    }

    private static bool Fits(Shape shape, int[] areas) {
        int needed = 0;

        for (int i = 0; i < shape.Counts.Length; i++)
            needed += shape.Counts[i] * areas[i];

        return needed <= shape.W * shape.H;
    }
}
