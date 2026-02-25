// ReSharper disable InconsistentNaming
namespace Shared.Numerics;

using System;

internal interface IVector<in T, TSelf> : IEquatable<TSelf>, IComparable<TSelf>
    where TSelf : IVector<T, TSelf>
    where T : struct, IEquatable<T>, IComparable<T>
{
    static abstract TSelf Zero { get; }
    static abstract TSelf One { get; }
    
    static abstract TSelf operator +(TSelf a, TSelf b);
    
    static abstract TSelf operator -(TSelf a, TSelf b);
    
    static abstract TSelf operator *(TSelf a, TSelf b);
    static abstract TSelf operator *(T a, TSelf b);
    static abstract TSelf operator *(TSelf a, T b);
    
    static abstract TSelf operator /(TSelf a, TSelf b);
    static abstract TSelf operator /(TSelf a, T b);
    static abstract TSelf operator /(T a, TSelf b);
    
    static abstract TSelf Abs(TSelf value);
}

public readonly record struct Vector2Int(int x, int y) : IVector<int, Vector2Int>
{
    // Static constants
    public static Vector2Int Zero => new(0, 0);
    public static Vector2Int One => new(1, 1);

    public static Vector2Int Up => new(0, 1);
    public static Vector2Int Down => new(0, -1);
    public static Vector2Int Left => new(-1, 0);
    public static Vector2Int Right => new(1, 0);

    // Operators
    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.x + b.x, a.y + b.y);
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.x - b.x, a.y - b.y);

    public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new(a.x * b.x, a.y * b.y);
    public static Vector2Int operator *(Vector2Int a, int b) => new(a.x * b, a.y * b);
    public static Vector2Int operator *(int b, Vector2Int a) => a * b;

    public static Vector2Int operator /(Vector2Int a, Vector2Int b) => new(a.x / b.x, a.y / b.y);
    public static Vector2Int operator /(Vector2Int a, int b) => new(a.x / b, a.y / b);
    public static Vector2Int operator /(int a, Vector2Int b) => new(a / b.x, a / b.y);
    
    public static Vector2Int operator %(Vector2Int a, Vector2Int b) => new(a.x % b.x, a.y % b.y);
    public static Vector2Int operator %(Vector2Int a, int b) => new(a.x % b, a.y % b);
    public static Vector2Int operator %(int a, Vector2Int b) => new(a % b.x, a % b.y);
    
    // Mutations and queries
    public static Vector2Int Abs(Vector2Int value) => new(Math.Abs(value.x), Math.Abs(value.y));
    public static Vector2Int Min(Vector2Int a, Vector2Int b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    public static Vector2Int Max(Vector2Int a, Vector2Int b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    
    public int ManhattanDistance(Vector2Int other)
        => Math.Abs(x - other.x) + Math.Abs(y - other.y);

    public int DistanceSquared(Vector2Int other)
    {
        int dx = x - other.x;
        int dy = y - other.y;
        return dx * dx + dy * dy;
    }

    public Vector2Int Rotate(int degrees)
    {
        return degrees switch
        {
            90 => new Vector2Int(-y, x),
            180 => new Vector2Int(-x, -y),
            270 => new Vector2Int(y, -x),
            _ => throw new ArgumentException("Rotation must be 90, 180, or 270 degrees")
        };
    }
    
    // Equality and comparison
    public bool Equals(Vector2Int other) => x == other.x && y == other.y;
    public int CompareTo(Vector2Int other)
    {
        int cmp = x.CompareTo(other.x);
        return cmp != 0 ? cmp : y.CompareTo(other.y);
    }
    public override int GetHashCode() => HashCode.Combine(x, y);
}

public readonly record struct Vector2Long(long x, long y) : IVector<long, Vector2Long>
{
    // Static constants
    public static Vector2Long Zero => new(0, 0);
    public static Vector2Long One => new(1, 1);

    public static Vector2Long Up => new(0, 1);
    public static Vector2Long Down => new(0, -1);
    public static Vector2Long Left => new(-1, 0);
    public static Vector2Long Right => new(1, 0);

    // Operators
    public static Vector2Long operator +(Vector2Long a, Vector2Long b) => new(a.x + b.x, a.y + b.y);
    public static Vector2Long operator -(Vector2Long a, Vector2Long b) => new(a.x - b.x, a.y - b.y);

    public static Vector2Long operator *(Vector2Long a, Vector2Long b) => new(a.x * b.x, a.y * b.y);
    public static Vector2Long operator *(Vector2Long a, long b) => new(a.x * b, a.y * b);
    public static Vector2Long operator *(long b, Vector2Long a) => a * b;

    public static Vector2Long operator /(Vector2Long a, Vector2Long b) => new(a.x / b.x, a.y / b.y);
    public static Vector2Long operator /(Vector2Long a, long b) => new(a.x / b, a.y / b);
    public static Vector2Long operator /(long a, Vector2Long b) => new(a / b.x, a / b.y);
    
    public static Vector2Long operator %(Vector2Long a, Vector2Long b) => new(a.x % b.x, a.y % b.y);
    public static Vector2Long operator %(Vector2Long a, long b) => new(a.x % b, a.y % b);
    public static Vector2Long operator %(long a, Vector2Long b) => new(a % b.x, a % b.y);
    
    // Mutations and queries
    public static Vector2Long Abs(Vector2Long value) => new(Math.Abs(value.x), Math.Abs(value.y));
    public static Vector2Long Min(Vector2Long a, Vector2Long b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    public static Vector2Long Max(Vector2Long a, Vector2Long b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    
    public long ManhattanDistance(Vector2Long other)
        => Math.Abs(x - other.x) + Math.Abs(y - other.y);

    public long DistanceSquared(Vector2Long other)
    {
        long dx = x - other.x;
        long dy = y - other.y;
        return dx * dx + dy * dy;
    }

    public Vector2Long Rotate(int degrees)
    {
        return degrees switch
        {
            90 => new Vector2Long(-y, x),
            180 => new Vector2Long(-x, -y),
            270 => new Vector2Long(y, -x),
            _ => throw new ArgumentException("Rotation must be 90, 180, or 270 degrees")
        };
    }
    
    // Equality and comparison
    public bool Equals(Vector2Long other) => x == other.x && y == other.y;
    public int CompareTo(Vector2Long other)
    {
        int cmp = x.CompareTo(other.x);
        return cmp != 0 ? cmp : y.CompareTo(other.y);
    }
    public override int GetHashCode() => HashCode.Combine(x, y);
}