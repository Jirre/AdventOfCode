namespace AdventOfCode.Y2025.Day12;

using System;
using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("Christmas Tree Farm")]
class Solution : Solver {

    private record Area(int W, int H, int[] Counts);
    
    public object PartOne(string input) {
        string[] cells = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        int[] shapes = ParseShapes(cells[..^1]);
        Area[] areas = ParseAreas(cells[^1]);

        return areas.Count(area => Fits(area, shapes));
    }

    private static int[] ParseShapes(string[] cells) {
        int[] shapes = new int[cells.Length];

        for (int i = 0; i < cells.Length; i++) {
            int shape = 0;
            foreach (char ch in cells[i]) {
                if (ch == '#') shape++;
            }
            shapes[i] = shape;
        }

        return shapes;
    }

    private static Area[] ParseAreas(string cells) {
        string[] lines = cells.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Area[] areas = new Area[lines.Length];

        for (int i = 0; i < lines.Length; i++) {
            int[] numbers = Regex.Matches(lines[i], @"\d+")
                .Select(m => int.Parse(m.Value))
                .ToArray();

            int w = numbers[0];
            int h = numbers[1];
            int[] counts = numbers[2..];

            areas[i] = new Area(w, h, counts);
        }

        return areas;
    }

    private static bool Fits(Area area, int[] areas) {
        int needed = 0;

        for (int i = 0; i < area.Counts.Length; i++)
            needed += area.Counts[i] * areas[i];

        return needed <= area.W * area.H;
    }
}
