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

internal interface IVector2<T, TSelf> : IVector<T, TSelf>
    where TSelf : IVector2<T, TSelf>
    where T : struct, IEquatable<T>, IComparable<T>
{
    public T x { get; init; }
    public T y { get; init; }

    public T ManhattanDistance(TSelf other)
    {
        dynamic dx = x;
        dynamic dy = y;
        return (T)(Math.Abs(dx - other.x) + Math.Abs(dy - other.y));
    }

    public T DistanceSquared(TSelf other)
    {
        dynamic dx = x;
        dynamic dy = y;
        return (T)((dx - other.x) * (dx - other.x) + (dy - other.y) * (dy - other.y));
    }
    
    public TSelf Rotate(int degrees)
    {
        dynamic dx = x;
        dynamic dy = y;

        return degrees switch
        {
            90  => (TSelf)Activator.CreateInstance(typeof(TSelf), dy, -dx)!,
            180 => (TSelf)Activator.CreateInstance(typeof(TSelf), -dx, -dy)!,
            270 => (TSelf)Activator.CreateInstance(typeof(TSelf), -dy, dx)!,
            _ => throw new ArgumentException("Rotation must be 90, 180, or 270 degrees")
        };
    }
}

public readonly record struct Vector2Int(int x, int y) : IVector2<int, Vector2Int>
{
    // Static constants
    public static Vector2Int Zero => new(0, 0);
    public static Vector2Int One => new(1, 1);

    public static Vector2Int Up => new(0, -1);
    public static Vector2Int Down => new(0, 1);
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

    public static Vector2Int Abs(Vector2Int value) => new(Math.Abs(value.x), Math.Abs(value.y));
    
    // Equality and comparison
    public bool Equals(Vector2Int other) => x == other.x && y == other.y;
    public int CompareTo(Vector2Int other)
    {
        int cmp = x.CompareTo(other.x);
        return cmp != 0 ? cmp : y.CompareTo(other.y);
    }
    public override int GetHashCode() => HashCode.Combine(x, y);
}

public readonly record struct Vector2Long(long x, long y) : IVector2<long, Vector2Long>
{
    // Static constants
    public static Vector2Long Zero => new(0, 0);
    public static Vector2Long One => new(1, 1);

    public static Vector2Long Up => new(0, -1);
    public static Vector2Long Down => new(0, 1);
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

    public static Vector2Long Abs(Vector2Long value) => new(Math.Abs(value.x), Math.Abs(value.y));
    
    // Equality and comparison
    public bool Equals(Vector2Long other) => x == other.x && y == other.y;
    public int CompareTo(Vector2Long other)
    {
        int cmp = x.CompareTo(other.x);
        return cmp != 0 ? cmp : y.CompareTo(other.y);
    }
    public override int GetHashCode() => HashCode.Combine(x, y);
}