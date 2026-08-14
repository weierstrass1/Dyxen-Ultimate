using System.Numerics;

namespace DyxenUltimateRendering.Transform;

public class Vector2D(float x = 0, float y = 0) : Vector(x, y)
{
    public static ReadOnlyVector2D Zero { get; } = new(1, 1);
    public static ReadOnlyVector2D One { get; } = new(1, 1);
    public static ReadOnlyVector2D UnitX { get; } = new(1, 0);
    public static ReadOnlyVector2D UnitY { get; } = new(0, 1);
    public float X { get => this[0]; set => this[0] = value; }
    public float Y { get => this[1]; set => this[1] = value; }
    public static implicit operator Vector2(Vector2D vector)
    {
        return new(vector.X, vector.Y);
    }
    public static implicit operator Vector2D(Vector2 vector)
    {
        return new(vector.X, vector.Y);
    }
    public static Vector2D operator +(Vector2D a, Vector2D b)
    {
        return new(a.X + b.X, a.Y + b.Y);
    }
    public static Vector2D operator -(Vector2D a, Vector2D b)
    {
        return new(a.X - b.X, a.Y - b.Y);
    }
    public static Vector2D operator *(Vector2D a, Vector2D b)
    {
        return new(a.X * b.X, a.Y * b.Y);
    }
    public static Vector2D operator /(Vector2D a, Vector2D b)
    {
        return new(a.X / b.X, a.Y / b.Y);
    }
    public static Vector2D operator *(Vector2D a, float b)
    {
        return new(a.X * b, a.Y * b);
    }
    public static Vector2D operator *(float a, Vector2D b)
    {
        return b * a;
    }
    public static Vector2D operator /(Vector2D a, float b)
    {
        return new(a.X / b, a.Y / b);
    }
    public static Vector2D operator -(Vector2D a)
    {
        return new(-a.X, -a.Y);
    }
    public static float Cross(Vector2D a, Vector2D b)
    {
        return a.X * b.Y - a.Y * b.X;
    }
    public new Vector2D Normalize()
    {
        return base.Normalize().ToVector2D();
    }
    public new Vector2D Abs()
    {
        return base.Abs().ToVector2D();
    }
    public new Vector2D Clamp(float min, float max)
    {
        return base.Clamp(min, max).ToVector2D();
    }
    public void Deconstruct(out float x)
    {
        x = X;
    }
    public void Deconstruct(out float x, out float y)
    {
        x = X;
        y = Y;
    }
    public static Vector2D Lerp(Vector2D v1, Vector2D v2, float t)
    {
        return Vector.Lerp(v1, v2, t).ToVector2D();
    }
    public static Vector2D Max(Vector2D v1, Vector2D v2)
    {
        return Vector.Max(v1, v2).ToVector2D();
    }
    public static Vector2D Min(Vector2D v1, Vector2D v2)
    {
        return Vector.Min(v1, v2).ToVector2D();
    }
    public static implicit operator (float x, float y)(Vector2D v)
    {
        return (v.X, v.Y);
    }
    public static implicit operator Vector2D((float, float, float) v)
    {
        return new (v.Item1, v.Item2);
    }
    public Vector2D Clone()
    {
        return new(X, Y);
    }
}
