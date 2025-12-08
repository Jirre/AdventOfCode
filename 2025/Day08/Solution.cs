namespace AdventOfCode.Y2025.Day08;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Playground")]
class Solution : Solver {

    private readonly record struct V3(int X, int Y, int Z);
    
    public object PartOne(string input)
    {
        List<V3> points = ParseCoordinates(input);
        List<Pair> pairs = GetAllPairs(points);
        pairs.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        UnionHandler unions = new UnionHandler(points.Count);
        int connections = Math.Min(1000, pairs.Count);
        for (int i = 0; i < connections; i++)
        {
            Pair pair = pairs[i];
            unions.Join(pair.A, pair.B);
        }
        
        List<int> sizes = unions.GetUnionSizes();
        
        int[] top3 = sizes.OrderByDescending(x => x).Take(3).ToArray();

        return top3[0] * top3[1] * top3[2];
    }

    public object PartTwo(string input)
    {
        List<V3> points = ParseCoordinates(input);
        List<Pair> pairs = GetAllPairs(points);
        pairs.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        UnionHandler unions = new UnionHandler(points.Count);
        int groups = points.Count;
        V3 lastA = default;
        V3 lastB = default;

        foreach (Pair pair in pairs)
        {
            // If pairs are a part of the same union already, skip
            if (!unions.Join(pair.A, pair.B)) continue;
            groups--;
            
            lastA = points[pair.A];
            lastB = points[pair.B];

            // Stop when fully connected
            if (groups == 1)
                break;
        }
        
        return (long)lastA.X * (long)lastB.X;
    }
    
    private static List<V3> ParseCoordinates(string input)
    {
        return input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                string[] parts = line.Split(',');
                return new V3(
                    int.Parse(parts[0]),
                    int.Parse(parts[1]),
                    int.Parse(parts[2])
                );
            })
            .ToList();
    }

    /// <summary>
    /// Get the distance between nodes
    /// (no need to sqrt it as we don't actively need the accurate distance, just an ordering based on distance)
    /// </summary>
    private static long GetDistanceSquared(V3 a, V3 b)
    {
        long dx = a.X - b.X;
        long dy = a.Y - b.Y;
        long dz = a.Z - b.Z;
        return dx * dx + dy * dy + dz * dz;
    }

    private readonly struct Pair(int a, int b, long distance)
    {
        public readonly int A = a;
        public readonly int B = b;
        public readonly long Distance = distance;
    }

    /// <summary>
    /// Get for each point all distances to all other points without doing duplicate work
    /// </summary>
    private static List<Pair> GetAllPairs(List<V3> points)
    {
        List<Pair> result = [];
        int pointCount = points.Count;

        for (int i = 0; i < pointCount; i++)
        for (int j = i + 1; j < pointCount; j++)
            result.Add(new Pair(i, j, GetDistanceSquared(points[i], points[j])));

        return result;
    }
    
    private class UnionHandler
    {
        private readonly int[] _parent;
        private readonly int[] _size;

        public UnionHandler(int n)
        {
            _parent = new int[n];
            _size = new int[n];
            for (int i = 0; i < n; i++)
            {
                _parent[i] = i;
                _size[i] = 1;
            }
        }

        /// <summary>
        /// Get the root parent node of the union this node is a part of
        /// </summary>
        private int Find(int x)
        {
            if (_parent[x] != x)
                _parent[x] = Find(_parent[x]);
            return _parent[x];
        }

        /// <summary>
        /// Join two nodes together in a single union based on the node-ids
        /// If both nodes are part of two different unions they merge into a single new union
        /// If both nodes are already part of the same union, do nothing
        /// </summary>
        public bool Join(int a, int b)
        {
            int ra = Find(a);
            int rb = Find(b);
            if (ra == rb)
                return false;

            if (_size[ra] < _size[rb])
                (ra, rb) = (rb, ra);

            _parent[rb] = ra;
            _size[ra] += _size[rb];
            return true;
        }

        /// <summary>
        /// Get the number of nodes for each union group
        /// </summary>
        public List<int> GetUnionSizes()
        {
            Dictionary<int, int> map = new Dictionary<int, int>();
            for (int i = 0; i < _parent.Length; i++)
            {
                int r = Find(i);
                map.TryAdd(r, 0);
                map[r]++;
            }
            return map.Values.ToList();
        }
    }
}