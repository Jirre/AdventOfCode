namespace AdventOfCode.Y2025.Day11;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Reactor")]
class Solution : Solver {

    public object PartOne(string input) {
        Dictionary<string, List<string>> graph = ParseGraph(input);
        return CountPaths("you", "out", graph, new HashSet<string>());
    }

    public object PartTwo(string input) {
        Dictionary<string, List<string>> graph = ParseGraph(input);
        string[] required = { "dac", "fft" };
        return CountPathsWithRequirements("svr", "out", required, graph);
    }
    
    private static Dictionary<string, List<string>> ParseGraph(string input) =>
        input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
             .Select(line => line.Split(':', 2))
             .ToDictionary(
                p => p[0].Trim(),
                p => p[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
             );
    
    private static long CountPaths(string current, string target, Dictionary<string, List<string>> graph, HashSet<string> visited) {
        if (current == target)
            return 1;

        if (!visited.Add(current))
            return 0;

        long total = 0;

        if (!graph.TryGetValue(current, out List<string> nodes)) 
            return total;

        foreach (string node in nodes) {
            total += CountPaths(node, target, graph, new HashSet<string>(visited));
        }

        return total;
    }
    
    private readonly record struct CacheKey(string Node, int Mask);
    
    private static long CountPathsWithRequirements(
        string start,
        string target,
        string[] required,
        Dictionary<string, List<string>> graph
    ) {
        Dictionary<string, int> requiredIndex = new();
        for (int i = 0; i < required.Length; i++)
            requiredIndex[required[i]] = i;

        int requiredMask = (1 << required.Length) - 1;

        Dictionary<CacheKey, long> cache = new();

        return CountValidPathsRecursive(
            start,
            target,
            visitedMask: 0,
            requiredIndex,
            requiredMask,
            graph,
            cache
        );
    }
    
    private static long CountValidPathsRecursive(
        string node,
        string target,
        int visitedMask,
        Dictionary<string, int> requiredIndex,
        int requiredMask,
        Dictionary<string, List<string>> graph,
        Dictionary<CacheKey, long> cache
    ) {
        // Update mask if this node is required
        if (requiredIndex.TryGetValue(node, out int bit))
            visitedMask |= (1 << bit);

        // If we reached the target, only count if all required nodes were seen
        if (node == target)
            return visitedMask == requiredMask ? 1 : 0;

        CacheKey key = new(node, visitedMask);

        if (cache.TryGetValue(key, out long cached))
            return cached;

        long total = 0;

        if (graph.TryGetValue(node, out List<string> nodes)) {
            foreach (string n in nodes) {
                total += CountValidPathsRecursive(
                    n,
                    target,
                    visitedMask,
                    requiredIndex,
                    requiredMask,
                    graph,
                    cache
                );
            }
        }

        cache[key] = total;
        return total;
    }
}
