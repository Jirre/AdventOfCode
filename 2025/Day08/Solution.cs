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
        int n = points.Count;
        
        List<Pair> pairs = GetAllPairs(points);
        pairs.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        UnionHandler unions = new UnionHandler(n);
        int connections = Math.Min(1000, pairs.Count);
        for (int i = 0; i < connections; i++)
        {
            (int i1, int i2, _) = pairs[i];
            unions.Join(i1, i2);
        }
        
        List<int> sizes = unions.GetComponentSizes();
        
        int[] top3 = sizes.OrderByDescending(x => x).Take(3).ToArray();

        return top3[0] * top3[1] * top3[2];
    }

    public object PartTwo(string input)
    {
        List<V3> points = ParseCoordinates(input);
        int n = points.Count;
        
        List<Pair> pairs = GetAllPairs(points);
        pairs.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        UnionHandler unions = new UnionHandler(n);
        int components = n;
        V3 lastA = default;
        V3 lastB = default;

        foreach ((int i1, int i2, long _) in pairs)
        {
            // If pairs are identical, skip
            if (!unions.Join(i1, i2)) continue;
            components--;
            
            lastA = points[i1];
            lastB = points[i2];

            // Stop when fully connected
            if (components == 1)
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

    private static long GetDistanceSquared(V3 a, V3 b)
    {
        long dx = a.X - b.X;
        long dy = a.Y - b.Y;
        long dz = a.Z - b.Z;
        return dx * dx + dy * dy + dz * dz;
    }

    private record struct Pair(int I1, int I2, long Distance);

    private static List<Pair> GetAllPairs(List<V3> points)
    {
        List<Pair> result = new List<Pair>();
        int n = points.Count;

        for (int i = 0; i < n; i++)
        for (int j = i + 1; j < n; j++)
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

        private int Find(int x)
        {
            if (_parent[x] != x)
                _parent[x] = Find(_parent[x]);
            return _parent[x];
        }

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

        public List<int> GetComponentSizes()
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