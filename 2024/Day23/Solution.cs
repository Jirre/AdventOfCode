using System.Collections.Generic;

namespace AdventOfCode.Y2024.Day23;

using System;
using System.Linq;

using Graph = Dictionary<string, HashSet<string>>;
using Network = HashSet<string>;

[ProblemName("LAN Party")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        Graph graph = ParseGraph(input);
        Network nodes = [.. graph.Keys];

        nodes = Connect(graph, nodes);
        nodes = Connect(graph, nodes);

        return nodes.Count(network =>
            GetNodes(network).Any(member => member.StartsWith('t')));
    }

    public object PartTwo(string input)
    {
        Graph graph = ParseGraph(input);
        Network best = [];

        FindLargestNetwork(
            graph,
            [],
            [.. graph.Keys],
            [],
            ref best);

        return string.Join(",", best.OrderBy(node => node));
    }

    private static void FindLargestNetwork(
        Graph graph,
        Network current,
        Network candidates,
        Network excluded,
        ref Network best)
    {
        if (current.Count + candidates.Count <= best.Count)
            return;

        if (candidates.Count == 0 && excluded.Count == 0)
        {
            best = [.. current];
            return;
        }

        string pivot = candidates
            .Concat(excluded)
            .MaxBy(node => graph[node].Count(candidates.Contains))!;

        Network remaining = [.. candidates.Where(node => !graph[pivot].Contains(node))];

        foreach (string node in remaining)
        {
            current.Add(node);

            FindLargestNetwork(
                graph,
                current,
                candidates.Intersect(graph[node]).ToHashSet(),
                excluded.Intersect(graph[node]).ToHashSet(),
                ref best);

            current.Remove(node);
            candidates.Remove(node);
            excluded.Add(node);
        }
    }

    private static Network Connect(Graph graph, Network nodes)
    {
        return nodes
            .AsParallel()
            .SelectMany(node => ExtendNetwork(graph, node))
            .ToHashSet();
    }

    private static IEnumerable<string> ExtendNetwork(
        Graph graph,
        string network)
    {
        string[] nodes = GetNodes(network);

        return GetNeighbours(graph, nodes)
            .Where(neighbour => nodes.All(node =>
                graph[neighbour].Contains(node)))
            .Select(neighbour => AddNode(nodes, neighbour));
    }

    private static IEnumerable<string> GetNeighbours(
        Graph graph,
        string[] nodes)
    {
        Network existing = [.. nodes];

        return nodes
            .SelectMany(node => graph[node])
            .Where(neighbour => !existing.Contains(neighbour))
            .Distinct();
    }

    private static string[] GetNodes(string network) =>
        network.Split(',');

    private static string AddNode(
        string[] nodes,
        string item)
    {
        int index = Array.BinarySearch(nodes, item);

        if (index < 0)
            index = ~index;

        string[] result = new string[nodes.Length + 1];

        Array.Copy(nodes, 0, result, 0, index);
        result[index] = item;
        Array.Copy(nodes, index, result, index + 1, nodes.Length - index);

        return string.Join(",", result);
    }

    private static Graph ParseGraph(string input)
    {
        Graph graph = [];

        foreach (string line in input.Split('\n'))
        {
            if (line.Length == 0)
                continue;

            string[] nodes = line.Split('-');

            if (!graph.TryGetValue(nodes[0], out Network a))
                graph[nodes[0]] = a = [];

            if (!graph.TryGetValue(nodes[1], out Network b))
                graph[nodes[1]] = b = [];

            a.Add(nodes[1]);
            b.Add(nodes[0]);
        }

        return graph;
    }
}